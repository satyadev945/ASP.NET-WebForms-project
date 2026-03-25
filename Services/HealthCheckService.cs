using System;
using FIlms.DTOs;

namespace FIlms.Services
{
    /// <summary>
    /// Service for health check operations
    /// </summary>
    public class HealthCheckService
    {
        /// <summary>
        /// Performs a health check
        /// </summary>
        /// <returns>Health check response</returns>
        public HealthCheckResponse PerformHealthCheck()
        {
            try
            {
                // Perform any necessary health checks here
                // For example: database connectivity, external service availability, etc.
                
                // Log the health check
                LogHealthCheck();

                return new HealthCheckResponse
                {
                    Message = "Service is healthy and running",
                    Timestamp = DateTime.UtcNow.ToString("o")
                };
            }
            catch (Exception ex)
            {
                // Log the error
                LogError(ex);

                return new HealthCheckResponse
                {
                    Message = $"Health check failed: {ex.Message}",
                    Timestamp = DateTime.UtcNow.ToString("o")
                };
            }
        }

        /// <summary>
        /// Logs health check request
        /// </summary>
        private void LogHealthCheck()
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Health check performed successfully");
        }

        /// <summary>
        /// Logs error
        /// </summary>
        /// <param name="ex">Exception to log</param>
        private void LogError(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] ERROR: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }
}
