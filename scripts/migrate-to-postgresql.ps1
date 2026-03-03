# PostgreSQL Migration Script for Films Application (PowerShell)
# This script automates the migration process from SQL Server to PostgreSQL

param(
    [string]$PgHost = "localhost",
    [string]$PgPort = "5432",
    [string]$PgDatabase = "films",
    [string]$PgUser = "postgres",
    [string]$PgPassword = "postgres"
)

$ErrorActionPreference = "Stop"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Films Application - PostgreSQL Migration" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Configuration
$ProjectRoot = Split-Path -Parent $PSScriptRoot
$InfrastructureProject = Join-Path $ProjectRoot "src\Films.Infrastructure"
$WebProject = Join-Path $ProjectRoot "src\Films.Web"
$MigrationsDir = Join-Path $InfrastructureProject "Migrations"

Write-Host "Configuration:" -ForegroundColor Yellow
Write-Host "  Project Root: $ProjectRoot"
Write-Host "  PostgreSQL Host: $PgHost"
Write-Host "  PostgreSQL Port: $PgPort"
Write-Host "  PostgreSQL Database: $PgDatabase"
Write-Host "  PostgreSQL User: $PgUser"
Write-Host ""

# Function to print colored messages
function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Error-Custom {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Warning-Custom {
    param([string]$Message)
    Write-Host "⚠ $Message" -ForegroundColor Yellow
}

function Write-Info {
    param([string]$Message)
    Write-Host "ℹ $Message" -ForegroundColor Cyan
}

# Check if dotnet is installed
try {
    $dotnetVersion = dotnet --version
    Write-Success "dotnet CLI found (version $dotnetVersion)"
} catch {
    Write-Error-Custom "dotnet CLI is not installed. Please install .NET 8.0 SDK."
    exit 1
}

# Check if dotnet-ef is installed
$efTool = dotnet tool list -g | Select-String "dotnet-ef"
if (-not $efTool) {
    Write-Warning-Custom "dotnet-ef tool not found. Installing..."
    dotnet tool install --global dotnet-ef
    Write-Success "dotnet-ef tool installed"
} else {
    Write-Success "dotnet-ef tool found"
}

# Step 1: Clean the solution
Write-Host ""
Write-Info "Step 1: Cleaning the solution..."
Set-Location $ProjectRoot
dotnet clean | Out-Null
Write-Success "Solution cleaned"

# Step 2: Restore packages
Write-Host ""
Write-Info "Step 2: Restoring NuGet packages..."
dotnet restore | Out-Null
Write-Success "Packages restored"

# Step 3: Build the solution
Write-Host ""
Write-Info "Step 3: Building the solution..."
try {
    dotnet build --no-restore | Out-Null
    Write-Success "Solution built successfully"
} catch {
    Write-Error-Custom "Build failed. Please check the error messages above."
    exit 1
}

# Step 4: Remove old migrations
Write-Host ""
Write-Info "Step 4: Checking for existing migrations..."
if (Test-Path $MigrationsDir) {
    Write-Warning-Custom "Found existing migrations directory. Backing up..."
    $BackupDir = "$MigrationsDir.backup.$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    Move-Item $MigrationsDir $BackupDir
    Write-Success "Migrations backed up to: $BackupDir"
} else {
    Write-Info "No existing migrations found"
}

# Step 5: Check PostgreSQL connection
Write-Host ""
Write-Info "Step 5: Checking PostgreSQL connection..."
$psqlAvailable = Get-Command psql -ErrorAction SilentlyContinue
if ($psqlAvailable) {
    Write-Success "psql client found"
    
    # Set environment variable for password
    $env:PGPASSWORD = $PgPassword
    
    # Check if database exists
    $dbExists = psql -h $PgHost -p $PgPort -U $PgUser -lqt | Select-String $PgDatabase
    
    if ($dbExists) {
        Write-Warning-Custom "Database '$PgDatabase' already exists"
        $response = Read-Host "Do you want to drop and recreate it? (y/N)"
        if ($response -eq 'y' -or $response -eq 'Y') {
            psql -h $PgHost -p $PgPort -U $PgUser -c "DROP DATABASE IF EXISTS $PgDatabase;" | Out-Null
            psql -h $PgHost -p $PgPort -U $PgUser -c "CREATE DATABASE $PgDatabase;" | Out-Null
            Write-Success "Database recreated"
        }
    } else {
        Write-Info "Creating database '$PgDatabase'..."
        psql -h $PgHost -p $PgPort -U $PgUser -c "CREATE DATABASE $PgDatabase;" | Out-Null
        Write-Success "Database created"
    }
    
    # Clear password from environment
    Remove-Item Env:\PGPASSWORD
} else {
    Write-Warning-Custom "psql client not found. Skipping database creation."
    Write-Info "Please ensure the PostgreSQL database '$PgDatabase' exists"
}

# Step 6: Create new migrations
Write-Host ""
Write-Info "Step 6: Creating new PostgreSQL migrations..."
Set-Location $WebProject
try {
    dotnet ef migrations add InitialCreate --project $InfrastructureProject --startup-project $WebProject | Out-Null
    Write-Success "Migrations created successfully"
} catch {
    Write-Error-Custom "Failed to create migrations. Please check the error messages."
    exit 1
}

# Step 7: Apply migrations
Write-Host ""
Write-Info "Step 7: Applying migrations to database..."
try {
    dotnet ef database update --project $InfrastructureProject --startup-project $WebProject
    Write-Success "Migrations applied successfully"
} catch {
    Write-Error-Custom "Failed to apply migrations. Please check the error messages."
    exit 1
}

# Step 8: Verify database schema
if ($psqlAvailable) {
    Write-Host ""
    Write-Info "Step 8: Verifying database schema..."
    
    $env:PGPASSWORD = $PgPassword
    
    Write-Host ""
    Write-Host "Tables in database:" -ForegroundColor Yellow
    psql -h $PgHost -p $PgPort -U $PgUser -d $PgDatabase -c "\dt public.*"
    
    Write-Host ""
    Write-Host "Migration history:" -ForegroundColor Yellow
    psql -h $PgHost -p $PgPort -U $PgUser -d $PgDatabase -c "SELECT * FROM public.__ef_migrations_history;"
    
    Remove-Item Env:\PGPASSWORD
    Write-Success "Database schema verified"
} else {
    Write-Host ""
    Write-Warning-Custom "Step 8: Skipping database verification (psql not available)"
}

# Step 9: Run tests (optional)
Write-Host ""
$runTests = Read-Host "Do you want to run tests? (y/N)"
if ($runTests -eq 'y' -or $runTests -eq 'Y') {
    Write-Info "Running tests..."
    Set-Location $ProjectRoot
    try {
        dotnet test --no-build
        Write-Success "All tests passed"
    } catch {
        Write-Warning-Custom "Some tests failed. Please review the results."
    }
}

# Summary
Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Migration Summary" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Success "PostgreSQL migration completed successfully!"
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "  1. Update connection string in appsettings.json with your credentials"
Write-Host "  2. Review the migration files in: $MigrationsDir"
Write-Host "  3. Test the application: cd $WebProject; dotnet run"
Write-Host "  4. Review the migration guide: $ProjectRoot\POSTGRESQL_MIGRATION.md"
Write-Host ""
Write-Host "Connection string format:" -ForegroundColor Yellow
Write-Host "  Host=$PgHost;Port=$PgPort;Database=$PgDatabase;Username=$PgUser;Password=YOUR_PASSWORD"
Write-Host ""
Write-Success "Migration script completed!"
