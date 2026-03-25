using System;
using System.ComponentModel.DataAnnotations;

namespace FIlms.DTOs
{
    /// <summary>
    /// Data Transfer Object for health check response
    /// </summary>
    public class HealthResponseDto
    {
        /// <summary>
        /// Health status message
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        /// Timestamp of the health check in ISO 8601 format
        /// </summary>
        [Required]
        public string Timestamp { get; set; }
    }
}
