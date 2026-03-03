# PostgreSQL Migration Guide for Films Application

## Overview
This document describes the migration of the Films application from SQL Server to PostgreSQL 16.

## Migration Summary

### Changes Made

#### 1. Package Dependencies Updated
**File:** `Films.Infrastructure/Films.Infrastructure.csproj`

**Removed:**
- `Microsoft.EntityFrameworkCore.SqlServer` Version 8.0.0

**Added:**
- `Npgsql.EntityFrameworkCore.PostgreSQL` Version 8.0.0
- `EFCore.NamingConventions` Version 8.0.0
- `Microsoft.EntityFrameworkCore.Relational` Version 8.0.0

#### 2. DbContext Configuration Updated
**File:** `Films.Infrastructure/Data/FilmsDbContext.cs`

**Changes:**
- Set default schema to "public" for PostgreSQL
- Converted all table names to snake_case (e.g., "Films" → "films")
- Converted all column names to snake_case (e.g., "FirstName" → "first_name")
- Updated DateTime properties to use `timestamp without time zone` column type
- Maintained all entity relationships and foreign key configurations

**Entity Mappings:**
- Film → films
- Actor → actors
- Director → directors
- User → users
- Sex → sex
- TypeUser → type_user
- Right → rights
- UserRight → user_rights
- RefAF → ref_af
- RefDAF → ref_daf

#### 3. Program.cs Configuration Updated
**File:** `Films.Web/Program.cs`

**Changes:**
- Replaced `UseSqlServer()` with `UseNpgsql()`
- Added snake_case naming convention with `UseSnakeCaseNamingConvention()`
- Configured migrations history table: `__ef_migrations_history` in "public" schema
- Added retry policy with 5 max retries and 30-second delay
- Enabled sensitive data logging for development environment
- Enabled detailed errors for development environment

#### 4. Connection Strings Updated
**Files:** 
- `Films.Web/appsettings.json`
- `Films.Web/appsettings.Development.json`

**SQL Server Format (Old):**
```
Server=localhost;Database=films;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True
```

**PostgreSQL Format (New):**
```
Host=localhost;Port=5432;Database=films;Username=postgres;Password=postgres;Pooling=true;Minimum Pool Size=0;Maximum Pool Size=100;Connection Lifetime=0;Connection Idle Lifetime=300;Timeout=30;Command Timeout=30
```

## Database Schema Changes

### Naming Convention
All database objects now use snake_case naming convention:
- Tables: `films`, `actors`, `directors`, `users`, etc.
- Columns: `first_name`, `last_name`, `created_date`, etc.

### Data Type Mappings

| SQL Server Type | PostgreSQL Type |
|----------------|-----------------|
| datetime2 | timestamp without time zone |
| nvarchar(n) | character varying(n) |
| int | integer |
| bit | boolean |

### DateTime Handling
- All DateTime properties are stored as `timestamp without time zone`
- Application uses `DateTime.UtcNow` for consistency
- No timezone conversion is performed at the database level

## Migration Steps

### Prerequisites
1. PostgreSQL 16 installed and running
2. .NET 8.0 SDK installed
3. EF Core tools installed: `dotnet tool install --global dotnet-ef`

### Step 1: Update Connection String
Update the connection string in `appsettings.json` with your PostgreSQL credentials:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-host;Port=5432;Database=films;Username=your-username;Password=your-password"
  }
}
```

### Step 2: Create Database
```sql
CREATE DATABASE films;
```

### Step 3: Remove Old Migrations (if any)
```bash
cd Films.Web
rm -rf Migrations/
```

### Step 4: Create New PostgreSQL Migrations
```bash
dotnet ef migrations add InitialCreate --project ../Films.Infrastructure --startup-project .
```

### Step 5: Apply Migrations
```bash
dotnet ef database update --project ../Films.Infrastructure --startup-project .
```

### Step 6: Verify Database Schema
Connect to PostgreSQL and verify:
```sql
\dt public.*
\d+ public.films
```

## Testing Checklist

- [ ] Database connection successful
- [ ] All tables created with correct snake_case names
- [ ] All columns created with correct snake_case names
- [ ] Foreign key relationships established
- [ ] CRUD operations working for all entities
- [ ] DateTime values stored and retrieved correctly
- [ ] Application logging working correctly
- [ ] Session management working
- [ ] Authentication working

## Rollback Plan

If you need to rollback to SQL Server:

1. Restore the original `.csproj` file with SQL Server packages
2. Restore the original `FilmsDbContext.cs` without snake_case naming
3. Restore the original `Program.cs` with `UseSqlServer()`
4. Restore the original connection strings
5. Restore SQL Server migrations

## Performance Considerations

### Connection Pooling
- Minimum Pool Size: 0
- Maximum Pool Size: 100
- Connection Idle Lifetime: 300 seconds

### Retry Policy
- Max Retry Count: 5
- Max Retry Delay: 30 seconds

### Indexing Recommendations
Consider adding indexes on frequently queried columns:
```sql
CREATE INDEX idx_films_title ON public.films(title);
CREATE INDEX idx_films_year ON public.films(year);
CREATE INDEX idx_actors_last_name ON public.actors(last_name);
CREATE INDEX idx_users_username ON public.users(username);
CREATE INDEX idx_users_email ON public.users(email);
```

## Known Limitations

1. **Case Sensitivity**: PostgreSQL is case-sensitive for unquoted identifiers. The snake_case naming convention handles this automatically.

2. **DateTime Precision**: PostgreSQL `timestamp` has microsecond precision, while SQL Server `datetime2` has 100-nanosecond precision.

3. **String Comparison**: PostgreSQL string comparison is case-sensitive by default. Use `ILIKE` for case-insensitive comparisons.

## Troubleshooting

### Connection Issues
- Verify PostgreSQL is running: `sudo systemctl status postgresql`
- Check PostgreSQL logs: `/var/log/postgresql/postgresql-16-main.log`
- Verify firewall allows port 5432

### Migration Issues
- Clear EF Core cache: `rm -rf ~/.nuget/packages/npgsql*`
- Rebuild solution: `dotnet clean && dotnet build`
- Check migration history: `SELECT * FROM public.__ef_migrations_history;`

### Performance Issues
- Analyze query plans: `EXPLAIN ANALYZE SELECT ...`
- Check connection pool: Monitor active connections
- Review indexes: `\di public.*`

## Additional Resources

- [Npgsql Documentation](https://www.npgsql.org/efcore/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/16/)
- [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)

## Support

For issues or questions, contact the development team or refer to the project documentation.
