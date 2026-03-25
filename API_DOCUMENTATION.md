# Films API - Health Endpoint

## Overview
This document describes the newly added Health Check REST API endpoint for the Films application.

## New REST API Endpoint

### Health Check Endpoint

**Endpoint:** `GET /health`

**Description:** Returns the health status of the application. This endpoint is useful for monitoring and load balancer health checks.

**Authentication:** Required (uses Forms Authentication)

**Request:**
- Method: GET
- URL: `/health`
- Headers: 
  - Authentication cookie required (Forms Authentication)

**Response:**

Success (200 OK):
```json
{
  "message": "Service is healthy and running",
  "timestamp": "2024-01-15T10:30:45.123Z"
}
```

Error (500 Internal Server Error):
```json
{
  "message": "Service health check failed: [error details]",
  "timestamp": "2024-01-15T10:30:45.123Z"
}
```

**Response Fields:**
- `message` (string): Health status message
- `timestamp` (string): ISO 8601 formatted timestamp of the health check

## Implementation Details

### Architecture
The health endpoint follows ASP.NET Web API patterns integrated into the existing ASP.NET Web Forms application:

1. **Controller Layer** (`Controllers/HealthController.cs`)
   - Handles HTTP requests
   - Implements error handling
   - Returns appropriate HTTP status codes
   - Includes logging for monitoring

2. **DTO Layer** (`DTOs/HealthResponseDto.cs`)
   - Defines the response structure
   - Includes data validation attributes

3. **Configuration** (`App_Start/WebApiConfig.cs`)
   - Configures Web API routing
   - Sets up JSON formatting

### Files Added/Modified

**New Files:**
- `App_Start/WebApiConfig.cs` - Web API configuration
- `App_Start/RouteConfig.cs` - Routing configuration
- `App_Start/BundleConfig.cs` - Bundle configuration
- `App_Start/AuthConfig.cs` - Authentication configuration
- `Controllers/HealthController.cs` - Health check controller
- `DTOs/HealthResponseDto.cs` - Response DTO

**Modified Files:**
- `Global.asax.cs` - Added Web API initialization
- `Web.config` - Added Web API handlers
- `FIlms.csproj` - Added Web API package references
- `packages.config` - Added Web API NuGet packages

### Dependencies Added
- Microsoft.AspNet.WebApi (5.2.7)
- Microsoft.AspNet.WebApi.Client (5.2.7)
- Microsoft.AspNet.WebApi.Core (5.2.7)
- Microsoft.AspNet.WebApi.WebHost (5.2.7)
- Newtonsoft.Json (12.0.3)

## Usage Examples

### Using cURL
```bash
# With authentication cookie
curl -X GET http://localhost:64830/health \
  -H "Cookie: .ASPXAUTH=your-auth-cookie-here"
```

### Using Postman
1. Set method to GET
2. Enter URL: `http://localhost:64830/health`
3. Add authentication cookie in Headers or use Postman's cookie manager
4. Send request

### Using JavaScript (Fetch API)
```javascript
fetch('/health', {
  method: 'GET',
  credentials: 'include' // Include cookies
})
.then(response => response.json())
.then(data => {
  console.log('Health Status:', data.message);
  console.log('Timestamp:', data.timestamp);
})
.catch(error => console.error('Health check failed:', error));
```

## Testing

### Manual Testing
1. Start the application
2. Log in to get an authentication cookie
3. Make a GET request to `/health`
4. Verify the response contains `message` and `timestamp` fields
5. Verify the timestamp is in ISO 8601 format

### Expected Behavior
- **Authenticated Request:** Returns 200 OK with health status
- **Unauthenticated Request:** Returns 401 Unauthorized (redirects to login)
- **Server Error:** Returns 500 Internal Server Error with error details

## Monitoring Integration

This endpoint can be integrated with:
- Load balancers (AWS ELB, Azure Load Balancer, etc.)
- Monitoring tools (Nagios, Prometheus, etc.)
- Container orchestration (Kubernetes liveness/readiness probes)
- APM tools (New Relic, Application Insights, etc.)

## Security Considerations

1. **Authentication Required:** The endpoint requires Forms Authentication to prevent unauthorized access
2. **Error Information:** Error messages are logged but sanitized in responses
3. **Rate Limiting:** Consider implementing rate limiting for production use

## Future Enhancements

Potential improvements for the health endpoint:
- Add database connectivity check
- Include system metrics (CPU, memory usage)
- Add dependency health checks (external APIs, services)
- Implement different health levels (healthy, degraded, unhealthy)
- Add optional detailed health report with query parameter
- Remove authentication requirement for basic health checks (if needed for load balancers)

## Troubleshooting

### Common Issues

**Issue:** 404 Not Found
- **Cause:** Web API routing not configured properly
- **Solution:** Verify `GlobalConfiguration.Configure(WebApiConfig.Register)` is called in `Global.asax.cs`

**Issue:** 401 Unauthorized
- **Cause:** No authentication cookie present
- **Solution:** Log in first or remove `[Authorize]` attribute for public health checks

**Issue:** 500 Internal Server Error
- **Cause:** Application error or misconfiguration
- **Solution:** Check application logs and verify all dependencies are properly configured

## Contact

For questions or issues related to this endpoint, please contact the development team.
