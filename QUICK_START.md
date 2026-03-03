# Quick Start Guide - PostgreSQL Migration

## Prerequisites
- PostgreSQL 16 installed and running
- .NET 8.0 SDK installed
- EF Core tools: `dotnet tool install --global dotnet-ef`

## Quick Migration (Automated)

### Linux/macOS
```bash
cd scripts
chmod +x migrate-to-postgresql.sh
./migrate-to-postgresql.sh
```

### Windows
```powershell
cd scripts
.\migrate-to-postgresql.ps1
```

## Manual Migration Steps

### 1. Update Connection String
Edit `src/Films.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=films;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

### 2. Create Database
```sql
CREATE DATABASE films;
```

### 3. Create and Apply Migrations
```bash
cd src/Films.Web
dotnet ef migrations add InitialCreate --project ../Films.Infrastructure
dotnet ef database update --project ../Films.Infrastructure
```

### 4. Run Application
```bash
dotnet run
```

## Verify Migration

### Check Tables
```sql
\c films
\dt public.*
```

### Check Migration History
```sql
SELECT * FROM public.__ef_migrations_history;
```

## Key Changes Made

1. **Packages**: SQL Server → PostgreSQL (Npgsql)
2. **Naming**: PascalCase → snake_case
3. **Connection**: SQL Server format → PostgreSQL format
4. **DateTime**: datetime2 → timestamp without time zone
5. **Schema**: Default → public

## Common Issues

### Connection Failed
- Check PostgreSQL is running: `sudo systemctl status postgresql`
- Verify credentials in connection string
- Check firewall allows port 5432

### Migration Failed
- Clear EF cache: `rm -rf ~/.nuget/packages/npgsql*`
- Rebuild: `dotnet clean && dotnet build`

### Case Sensitivity
- PostgreSQL is case-sensitive
- Use snake_case for all identifiers
- EFCore.NamingConventions handles this automatically

## Testing

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/Films.Tests.Unit
```

## Rollback

If you need to rollback:
1. Restore backup of `.csproj` files
2. Restore backup of `FilmsDbContext.cs`
3. Restore backup of `Program.cs`
4. Restore backup of connection strings
5. Drop PostgreSQL database
6. Recreate SQL Server database

## Documentation

- Full Migration Guide: `POSTGRESQL_MIGRATION.md`
- Migration Reports: `MIGRATION_REPORTS.json`
- SQL Setup Script: `scripts/postgresql-setup.sql`

## Support

For detailed information, see `POSTGRESQL_MIGRATION.md`
