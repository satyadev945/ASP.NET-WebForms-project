# Films Management System

A modern .NET 8 application for managing films, actors, and users. This application has been migrated from ASP.NET Web Forms to ASP.NET Core 8 with Razor Pages using clean architecture principles.

## Project Structure

```
Films/
├── src/
│   ├── Films.Domain/           # Domain entities and interfaces
│   ├── Films.Application/      # Business logic and DTOs
│   ├── Films.Infrastructure/   # Data access and repositories
│   └── Films.Web/             # Web UI (Razor Pages)
├── tests/
│   ├── Films.UnitTests/       # Unit tests
│   └── Films.IntegrationTests/ # Integration tests
└── docs/                      # Documentation
```

## Features

- **Films Management**: Create, read, update, and delete films with detailed information
- **Actors Management**: Manage actors database with comprehensive information
- **Users Management**: User account management and permissions
- **Clean Architecture**: Separation of concerns with Domain, Application, Infrastructure, and Web layers
- **Entity Framework Core**: Modern ORM with code-first approach
- **Logging**: Structured logging with Serilog
- **Testing**: Comprehensive unit and integration tests

## Technology Stack

- **.NET 8**: Target framework
- **ASP.NET Core**: Web framework
- **Razor Pages**: Page-based programming model
- **Entity Framework Core**: Object-relational mapping
- **SQL Server**: Database
- **AutoMapper**: Object-to-object mapping
- **Serilog**: Structured logging
- **xUnit**: Testing framework
- **Bootstrap 5**: UI framework

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full instance)

### Setup

1. Clone the repository
2. Navigate to the project directory
3. Restore dependencies:
   ```bash
   dotnet restore
   ```

4. Update the connection string in `src/Films.Web/appsettings.json`

5. Apply database migrations:
   ```bash
   dotnet ef database update --project src/Films.Infrastructure --startup-project src/Films.Web
   ```

6. Build the solution:
   ```bash
   dotnet build
   ```

7. Run the application:
   ```bash
   dotnet run --project src/Films.Web
   ```

### Running Tests

```bash
# Run all tests
dotnet test

# Run unit tests only
dotnet test tests/Films.UnitTests

# Run integration tests only
dotnet test tests/Films.IntegrationTests
```

## Migration Notes

This application was successfully migrated from ASP.NET Web Forms 4.6 to .NET 8. Key changes include:

### Architecture Changes
- **Web Forms → Razor Pages**: Modern page-based programming model
- **Code-behind → Clean Architecture**: Proper separation of concerns
- **ViewState → Modern State Management**: Eliminated server-side state
- **Server Controls → HTML Helpers/Tag Helpers**: Modern HTML generation

### Technology Updates
- **Entity Framework 6 → EF Core 8**: Modern ORM with better performance
- **Web.config → appsettings.json**: Modern configuration system
- **Global.asax → Program.cs**: Modern application startup
- **Forms Authentication → ASP.NET Core Identity**: Modern authentication

### Benefits of Migration
- **Performance**: Faster startup and runtime performance
- **Cross-platform**: Runs on Windows, Linux, and macOS
- **Modern Development**: Latest C# features and development tools
- **Maintainability**: Clean architecture and modern patterns
- **Testability**: Comprehensive unit and integration testing

## API Documentation

The application provides a web interface for managing:

### Films
- List all films with search and filtering
- View film details
- Create new films
- Edit existing films
- Delete films

### Actors
- List all actors with search functionality
- View actor profiles
- Add new actors
- Edit actor information
- Remove actors

### Users
- User management interface
- Role and permission management
- User profile management

## Configuration

### Database Connection
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FilmsDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Logging
Configure logging levels in `appsettings.json`:
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For questions or issues, please create an issue in the project repository.