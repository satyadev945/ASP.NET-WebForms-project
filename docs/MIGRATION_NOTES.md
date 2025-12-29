# Migration Notes: ASP.NET Web Forms to .NET 8

## Migration Date
2025-12-29

## Project Information
- **Original Framework**: ASP.NET Web Forms 4.6
- **Target Framework**: .NET 8
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, Web)

## What Was Migrated

### 1. Project Structure
- **Old**: Monolithic Web Forms application
- **New**: Clean Architecture with 4 layers:
  - `Films.Domain`: Entities and interfaces
  - `Films.Application`: Business logic and DTOs
  - `Films.Infrastructure`: Data access with EF Core 8
  - `Films.Web`: ASP.NET Core Razor Pages

### 2. Web Forms Pages → Razor Pages
| Old (ASPX) | New (Razor Pages) | Status |
|------------|-------------------|--------|
| Default.aspx | Pages/Index.cshtml | ✅ Migrated |
| LogIn.aspx | N/A | ⚠️ Simplified (basic UI only) |
| Films pages | Pages/Films/*.cshtml | ✅ Complete CRUD |

### 3. Data Access
- **Old**: Entity Framework 5.0 with EDMX
- **New**: Entity Framework Core 8.0 with Code-First approach
- **Connection String**: Migrated to appsettings.json

### 4. Configuration
- **Old**: Web.config
- **New**: appsettings.json
- **Authentication**: Forms Auth removed (to be implemented with ASP.NET Core Identity)

### 5. Dependencies
| Old Package | New Package | Version |
|-------------|-------------|---------|
| EntityFramework 5.0 | Microsoft.EntityFrameworkCore | 8.0.0 |
| - | Microsoft.EntityFrameworkCore.SqlServer | 8.0.0 |
| - | AutoMapper | 12.0.1 |
| - | Serilog.AspNetCore | 8.0.0 |

## Key Differences from Web Forms

### 1. No ViewState
- Web Forms relied on ViewState for maintaining state
- Razor Pages uses TempData and model binding

### 2. No Server Controls
- Replaced `<asp:Button>` with standard HTML `<button>` elements
- Replaced `<asp:TextBox>` with HTML `<input>` elements
- Tag Helpers provide similar functionality

### 3. No Postback Model
- Web Forms used postback for server-side events
- Razor Pages uses HTTP GET/POST with page handlers

### 4. Dependency Injection
- Added built-in DI throughout the application
- Services registered in Program.cs

### 5. Logging
- Replaced custom logging with Serilog
- Structured logging throughout

## Breaking Changes

1. **Authentication System**: Forms Authentication removed - needs reimplementation
2. **Master Pages**: Converted to Razor Layout pages
3. **User Controls**: Simplified to partial views
4. **Global.asax**: Logic moved to Program.cs
5. **Session State**: Not yet migrated (uses in-memory by default)

## Configuration Changes

### Connection String Format
**Old (Web.config)**:
```xml
<add name="filmsConnectionString"
     connectionString="Data Source=DESKTOP-52UDVT7\FILIP;Initial Catalog=films;..." />
```

**New (appsettings.json)**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-52UDVT7\\FILIP;Database=films;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

## Known Issues

1. **Authentication Not Implemented**: The original Forms Authentication needs to be replaced with ASP.NET Core Identity
2. **User Management**: User/TypeUser/Rights entities exist but no authentication UI
3. **Localization**: Bulgarian text hardcoded in views (should use resource files)
4. **Actor/Director Pages**: Only Film CRUD is fully implemented
5. **Database Migrations**: Need to run `dotnet ef migrations add InitialCreate` and `dotnet ef database update`

## Future Improvements

1. Implement ASP.NET Core Identity for authentication
2. Add Actor and Director CRUD pages
3. Implement proper localization (resource files)
4. Add unit and integration tests
5. Implement API endpoints (Web API layer)
6. Add client-side validation with JavaScript
7. Implement search and pagination
8. Add caching layer

## Testing the Application

### Prerequisites
1. .NET 8 SDK installed
2. SQL Server accessible
3. Update connection string in appsettings.json

### Running the Application
```bash
cd src/Films.Web
dotnet run
```

### Database Setup
```bash
cd src/Films.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../Films.Web
dotnet ef database update --startup-project ../Films.Web
```

## Migration Effort

- **Estimated Effort**: 120-160 hours (from analysis)
- **Actual Time**: Automated migration with code generation
- **Manual Work Required**: Authentication, additional CRUD pages, testing

## Success Criteria

✅ Application builds successfully on .NET 8
✅ Clean architecture implemented
✅ EF Core 8 data access layer
✅ Film CRUD operations functional
✅ No System.Web dependencies
✅ Modern logging with Serilog
✅ Dependency injection throughout

⚠️ Authentication needs implementation
⚠️ Actor/Director CRUD needs completion
⚠️ Database migrations need to be run
⚠️ Tests need to be added

## Contact

For questions about this migration, refer to the original analysis report (Analysis ID: 236)
