using System;

namespace FIlms.DTOs
{
    /// <summary>
    /// Response DTO for health check endpoint
    /// </summary>
    public class HealthCheckResponse
    {
        /// <summary>
        /// Health check status message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Timestamp of the health check
        /// </summary>
        public string Timestamp { get; set; }
    }
}
