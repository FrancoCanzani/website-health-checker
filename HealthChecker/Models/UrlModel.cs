using System;
using System.ComponentModel.DataAnnotations;

namespace HealthChecker.Models
{
    public class UrlModel
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;  // To associate the URL with a specific user

        [Required]
        [Url]
        public string Url { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? LastChecked { get; set; }

        public bool LastStatus { get; set; }

        [Required]
        public string CheckFrequency { get; set; } = string.Empty;  // Can be "Daily", "Weekly", etc.

        public bool IsActive { get; set; }  // Flag to indicate if the URL check is active

        public int CheckInterval { get; set; }  // Interval in minutes, hours, or days

        public int FailureCount { get; set; }  // Count of consecutive failures

        public int SuccessCount { get; set; }  // Count of consecutive successes

        public DateTime? LastNotificationSent { get; set; }  // Timestamp of last notification sent
    }
}
