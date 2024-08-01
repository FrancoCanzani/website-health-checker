using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using HealthChecker.Services;

namespace HealthChecker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEmailService _emailService;

        public HealthCheckController(IHttpClientFactory httpClientFactory, IEmailService emailService)
        {
            _httpClientFactory = httpClientFactory;
            _emailService = emailService;

        }

        [HttpGet]
        public async Task<IActionResult> CheckHealth([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("URL parameter is required");
            }

            var healthCheckResult = await CheckWebsiteHealth(url);
            return Ok(healthCheckResult);
        }

        private async Task<object> CheckWebsiteHealth(string url)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(30);

                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var response = await client.GetAsync(url);

                stopwatch.Stop();

                _emailService.Send("francocanzani@gmail.com", "Site Down", $"The site {url} is down.");
                

                return new
                {
                    Url = url,
                    CheckDate = response.Headers.Date,
                    IsHealthy = response.IsSuccessStatusCode,
                    StatusCode = (int)response.StatusCode,
                    ResponseTime = stopwatch.ElapsedMilliseconds,
                    ContentType = response.Content.Headers.ContentType?.ToString(),
                    ServerHeader = response.Headers.Server.ToString(),
                    ContentLength = response.Content.Headers.ContentLength,
                    LastModified = response.Content.Headers.LastModified?.ToString()
                };
            }
            catch (HttpRequestException ex)
            {
                return new
                {
                    Url = url,
                    IsHealthy = false,
                    Error = ex.Message
                };
            }
            catch (TaskCanceledException)
            {
                return new
                {
                    Url = url,
                    IsHealthy = false,
                    Error = "Request timed out"
                };
            }
        }
    }
}