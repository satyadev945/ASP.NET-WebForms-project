using System;
using System.Web.Http;
using FIlms.DTOs;
using FIlms.Services;

namespace FIlms.Controllers
{
    /// <summary>
    /// API Controller for health checking
    /// </summary>
    [Authorize]
    public class HealthCheckController : ApiController
    {
        private readonly HealthCheckService _healthCheckService;

        /// <summary>
        /// Constructor
        /// </summary>
        public HealthCheckController()
        {
            _healthCheckService = new HealthCheckService();
        }

        /// <summary>
        /// GET: api/healthchecking
        /// Performs a health check and returns the status
        /// </summary>
        /// <returns>Health check response with message and timestamp</returns>
        [HttpGet]
        [Route("api/healthchecking")]
        public IHttpActionResult GetHealthCheck()
        {
            try
            {
                // Log the health check request
                System.Diagnostics.Debug.WriteLine($"Health check endpoint called at {DateTime.UtcNow}");

                // Perform health check using service
                var response = _healthCheckService.PerformHealthCheck();

                // Return appropriate response based on the result
                if (response.Message.Contains("failed"))
                {
                    return InternalServerError(new Exception(response.Message));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Health check endpoint error: {ex.Message}");
                
                // Return error response
                var errorResponse = new HealthCheckResponse
                {
                    Message = $"Health check failed: {ex.Message}",
                    Timestamp = DateTime.UtcNow.ToString("o")
                };

                return InternalServerError(new Exception(errorResponse.Message));
            }
        }
    }
}
