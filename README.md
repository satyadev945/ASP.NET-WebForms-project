# Films Web Application - PostgreSQL Migration Complete ✅

## Migration Status: SUCCESS

This application has been successfully migrated from **SQL Server** to **PostgreSQL 16**.

## What Changed?

### Database Provider
- ❌ Microsoft SQL Server
- ✅ PostgreSQL 16 with Npgsql

### Naming Convention
- ❌ PascalCase (Films, FirstName, CreatedDate)
- ✅ snake_case (films, first_name, created_date)

### Connection String
- ❌ `Server=localhost;Database=films;Integrated Security=True`
- ✅ `Host=localhost;Port=5432;Database=films;Username=postgres;Password=***`

### DateTime Handling
- ❌ SQL Server datetime2
- ✅ PostgreSQL timestamp without time zone

## Quick Start

### 1. Prerequisites
```bash
# Install PostgreSQL 16
sudo apt install postgresql-16  # Ubuntu/Debian
brew install postgresql@16      # macOS

# Install .NET 8.0 SDK
# Download from: https://dotnet.microsoft.com/download/dotnet/8.0

# Install EF Core tools
dotnet tool install --global dotnet-ef
```

### 2. Setup Database
```bash
# Start PostgreSQL
sudo systemctl start postgresql  # Linux
brew services start postgresql@16  # macOS

# Create database
sudo -u postgres psql -c "CREATE DATABASE films;"
```

### 3. Configure Connection
Edit `src/Films.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=films;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

### 4. Run Migration
```bash
# Option A: Automated (Recommended)
cd scripts
./migrate-to-postgresql.sh  # Linux/macOS
# OR
.\migrate-to-postgresql.ps1  # Windows

# Option B: Manual
cd src/Films.Web
dotnet ef migrations add InitialCreate --project ../Films.Infrastructure
dotnet ef database update --project ../Films.Infrastructure
```

### 5. Run Application
```bash
cd src/Films.Web
dotnet run
```

Visit: https://localhost:5001

## Project Structure

```
backendComp/
├── src/
│   ├── Films.Domain/          # Domain entities
│   ├── Films.Application/     # Business logic
│   ├── Films.Infrastructure/  # Data access (PostgreSQL)
│   └── Films.Web/            # Web API/UI
├── tests/
│   ├── Films.Tests.Unit/
│   └── Films.Tests.Integration/
├── scripts/
│   ├── postgresql-setup.sql           # Database setup
│   ├── migrate-to-postgresql.sh       # Migration script (Linux/macOS)
│   └── migrate-to-postgresql.ps1      # Migration script (Windows)
├── POSTGRESQL_MIGRATION.md            # Detailed migration guide
├── MIGRATION_REPORTS.json             # Pre/Post migration reports
├── QUICK_START.md                     # Quick reference
└── README.md                          # This file
```

## Database Schema

### Tables (snake_case)
- `films` - Film information
- `actors` - Actor information
- `directors` - Director information
- `users` - User accounts
- `sex` - Gender reference
- `type_user` - User type reference
- `rights` - Permission definitions
- `user_rights` - User-permission junction
- `ref_af` - Actor-film junction
- `ref_daf` - Director-film junction

### Key Features
- ✅ Snake_case naming convention
- ✅ Foreign key relationships
- ✅ Cascade delete where appropriate
- ✅ Indexes on frequently queried columns
- ✅ Timestamp tracking (created_date, modified_date)

## Testing

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/Films.Tests.Unit
dotnet test tests/Films.Tests.Integration

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## Documentation

| Document | Description |
|----------|-------------|
| [POSTGRESQL_MIGRATION.md](POSTGRESQL_MIGRATION.md) | Complete migration guide with detailed steps |
| [MIGRATION_REPORTS.json](MIGRATION_REPORTS.json) | Pre/post migration analysis reports |
| [QUICK_START.md](QUICK_START.md) | Quick reference for common tasks |
| [scripts/postgresql-setup.sql](scripts/postgresql-setup.sql) | Database setup script |

## Migration Summary

### Files Modified: 11
1. ✅ Films.Infrastructure.csproj - Package references
2. ✅ FilmsDbContext.cs - Entity configurations
3. ✅ Program.cs - DbContext setup
4. ✅ appsettings.json - Connection string
5. ✅ appsettings.Development.json - Dev connection string
6. ✅ POSTGRESQL_MIGRATION.md - Migration guide
7. ✅ MIGRATION_REPORTS.json - Reports
8. ✅ QUICK_START.md - Quick reference
9. ✅ postgresql-setup.sql - Setup script
10. ✅ migrate-to-postgresql.sh - Bash script
11. ✅ migrate-to-postgresql.ps1 - PowerShell script

### Changes Applied: 10
1. ✅ Package references updated
2. ✅ DbContext configuration updated
3. ✅ Connection strings converted
4. ✅ Table names converted to snake_case
5. ✅ Column names converted to snake_case
6. ✅ DateTime types mapped correctly
7. ✅ Default schema set to public
8. ✅ Migration history table configured
9. ✅ Retry policy added
10. ✅ Development logging enabled

## Troubleshooting

### Connection Issues
```bash
# Check PostgreSQL status
sudo systemctl status postgresql

# Check PostgreSQL logs
sudo tail -f /var/log/postgresql/postgresql-16-main.log

# Test connection
psql -h localhost -p 5432 -U postgres -d films
```

### Migration Issues
```bash
# Clear EF cache
rm -rf ~/.nuget/packages/npgsql*

# Rebuild solution
dotnet clean
dotnet build

# Check migration history
psql -h localhost -U postgres -d films -c "SELECT * FROM public.__ef_migrations_history;"
```

### Build Issues
```bash
# Restore packages
dotnet restore

# Clean and rebuild
dotnet clean
dotnet build --no-incremental
```

## Performance Tips

1. **Connection Pooling**: Already configured in connection string
2. **Indexes**: Run `scripts/postgresql-setup.sql` for recommended indexes
3. **Query Optimization**: Use `EXPLAIN ANALYZE` for slow queries
4. **Monitoring**: Enable PostgreSQL query logging for production

## Security Recommendations

1. **Credentials**: Use environment variables or Azure Key Vault
2. **SSL**: Enable SSL for production connections
3. **Firewall**: Restrict PostgreSQL port access
4. **Logging**: Disable sensitive data logging in production

## Rollback Plan

If you need to rollback to SQL Server:

1. Restore backup files from `Migrations.backup.*` directory
2. Update package references back to SQL Server
3. Update connection strings
4. Recreate SQL Server migrations
5. Apply migrations to SQL Server database

See [POSTGRESQL_MIGRATION.md](POSTGRESQL_MIGRATION.md) for detailed rollback steps.

## Support

For issues or questions:
1. Check [POSTGRESQL_MIGRATION.md](POSTGRESQL_MIGRATION.md) troubleshooting section
2. Review [MIGRATION_REPORTS.json](MIGRATION_REPORTS.json) for compliance details
3. Consult [Npgsql Documentation](https://www.npgsql.org/efcore/)
4. Contact development team

## License

[Your License Here]

## Contributors

- Development Team
- Database Migration Specialist

---

**Migration Completed**: 2024-01-15  
**PostgreSQL Version**: 16  
**EF Core Version**: 8.0.0  
**Npgsql Version**: 8.0.0  
**Status**: ✅ Production Ready
