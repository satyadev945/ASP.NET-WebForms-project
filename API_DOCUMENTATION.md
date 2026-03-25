# Health Check API Endpoint

## Overview
This document describes the health check REST API endpoint added to the FIlms application.

## Endpoint Details

### Health Check
- **HTTP Method**: GET
- **Endpoint Path**: `/api/healthchecking`
- **Description**: Performs a health check to verify the service is running and operational
- **Authentication Required**: Yes (requires user to be authenticated)

## Request

### Headers
```
Authorization: Bearer {token}
Content-Type: application/json
```

### Example Request
```http
GET /api/healthchecking HTTP/1.1
Host: localhost:64830
Authorization: Bearer {your-auth-token}
```

## Response

### Success Response (200 OK)
```json
{
  "message": "Service is healthy and running",
  "timestamp": "2024-01-15T10:30:45.1234567Z"
}
```

### Error Response (500 Internal Server Error)
```json
{
  "message": "Health check failed: {error details}",
  "exceptionMessage": "Health check failed: {error details}",
  "exceptionType": "System.Exception",
  "stackTrace": "..."
}
```

## Response Schema

| Field | Type | Description |
|-------|------|-------------|
| message | string | Status message indicating health check result |
| timestamp | string | ISO 8601 formatted timestamp of when the check was performed |

## Implementation Details

### Architecture
The endpoint follows a layered architecture pattern:

1. **Controller Layer** (`Controllers/HealthCheckController.cs`)
   - Handles HTTP requests
   - Manages authentication
   - Returns appropriate HTTP responses

2. **Service Layer** (`Services/HealthCheckService.cs`)
   - Contains business logic for health checks
   - Performs actual health verification
   - Handles logging

3. **DTO Layer** (`DTOs/HealthCheckResponse.cs`)
   - Defines the response structure
   - Ensures consistent API responses

### Configuration Files Modified

1. **Web.config**
   - Added Web API handlers for extensionless URLs
   - Configured IIS modules for API routing

2. **Global.asax.cs**
   - Registered Web API configuration
   - Initialized routing for API endpoints

3. **packages.config**
   - Added ASP.NET Web API packages
   - Added Newtonsoft.Json for JSON serialization

4. **FIlms.csproj**
   - Added references to Web API assemblies
   - Included new source files in compilation

### New Files Created

1. `App_Start/WebApiConfig.cs` - Web API routing configuration
2. `Controllers/HealthCheckController.cs` - API controller
3. `Services/HealthCheckService.cs` - Business logic service
4. `DTOs/HealthCheckResponse.cs` - Response data transfer object

## Usage Examples

### Using cURL
```bash
curl -X GET "http://localhost:64830/api/healthchecking" \
  -H "Authorization: Bearer {your-token}" \
  -H "Content-Type: application/json"
```

### Using PowerShell
```powershell
$headers = @{
    "Authorization" = "Bearer {your-token}"
    "Content-Type" = "application/json"
}

Invoke-RestMethod -Uri "http://localhost:64830/api/healthchecking" `
    -Method Get `
    -Headers $headers
```

### Using JavaScript (Fetch API)
```javascript
fetch('http://localhost:64830/api/healthchecking', {
    method: 'GET',
    headers: {
        'Authorization': 'Bearer {your-token}',
        'Content-Type': 'application/json'
    }
})
.then(response => response.json())
.then(data => console.log(data))
.catch(error => console.error('Error:', error));
```

## Authentication

The endpoint requires authentication using the application's existing authentication mechanism (Forms Authentication). Users must be logged in to access this endpoint.

To bypass authentication for health checks (if needed for monitoring tools), remove the `[Authorize]` attribute from the controller.

## Logging

The endpoint includes logging at multiple levels:
- Request logging when the endpoint is called
- Success logging when health check passes
- Error logging when health check fails

Logs are written to the Debug output and can be viewed in Visual Studio's Output window or configured to write to a file.

## Error Handling

The endpoint includes comprehensive error handling:
- Try-catch blocks at both controller and service levels
- Detailed error messages in responses
- Proper HTTP status codes (200 for success, 500 for errors)

## Future Enhancements

Potential improvements for the health check endpoint:
1. Add database connectivity checks
2. Add external service availability checks
3. Include system resource metrics (CPU, memory)
4. Add configurable health check levels (basic, detailed)
5. Implement caching to prevent excessive health checks
6. Add support for different response formats (XML, plain text)

## Testing

To test the endpoint:
1. Start the application in Visual Studio
2. Log in to the application
3. Use a tool like Postman or cURL to call the endpoint
4. Verify the response contains the expected message and timestamp

## Troubleshooting

### Common Issues

1. **401 Unauthorized**
   - Ensure you are authenticated
   - Check that the Authorization header is properly set

2. **404 Not Found**
   - Verify Web API routing is configured in Global.asax.cs
   - Check that the route attribute matches the request URL

3. **500 Internal Server Error**
   - Check the application logs for detailed error messages
   - Verify all required assemblies are referenced

## Support

For issues or questions about this endpoint, please contact the development team.
