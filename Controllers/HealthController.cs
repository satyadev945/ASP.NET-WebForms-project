using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FIlms.DTOs;

namespace FIlms.Controllers
{
    /// <summary>
    /// Health check controller for monitoring application status
    /// </summary>
    [RoutePrefix("health")]
    public class HealthController : ApiController
    {
        /// <summary>
        /// GET: /health
        /// Returns the health status of the application
        /// </summary>
        /// <returns>Health status response with message and timestamp</returns>
        [HttpGet]
        [Route("")]
        [Authorize] // Authentication required as per specification
        public IHttpActionResult GetHealthStatus()
        {
            try
            {
                // Log the health check request
                System.Diagnostics.Debug.WriteLine($"Health check requested at {DateTime.UtcNow}");

                // Create response DTO
                var response = new HealthResponseDto
                {
                    Message = "Service is healthy and running",
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };

                // Return 200 OK with the health status
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Health check failed: {ex.Message}");

                // Return 500 Internal Server Error
                var errorResponse = new HealthResponseDto
                {
                    Message = $"Service health check failed: {ex.Message}",
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };

                return Content(HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }
}
