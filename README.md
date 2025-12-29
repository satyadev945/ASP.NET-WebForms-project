# Films Management System - .NET 8

Successfully migrated from ASP.NET Web Forms 4.6 to .NET 8 with Clean Architecture.

## Quick Start

1. Update connection string in `src/Films.Web/appsettings.json`
2. Run migrations: `cd src/Films.Infrastructure && dotnet ef migrations add InitialCreate --startup-project ../Films.Web && dotnet ef database update --startup-project ../Films.Web`
3. Run app: `cd ../Films.Web && dotnet run`

## Structure

- `src/Films.Domain` - Entities and interfaces
- `src/Films.Application` - Business logic and DTOs
- `src/Films.Infrastructure` - EF Core data access
- `src/Films.Web` - Razor Pages UI

## Status

✅ Builds on .NET 8
✅ Film CRUD complete
⚠️ Authentication not implemented
⚠️ Actor/Director CRUD pending

See `docs/MIGRATION_NOTES.md` for details.
