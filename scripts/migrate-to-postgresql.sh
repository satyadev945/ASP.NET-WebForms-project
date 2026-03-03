#!/bin/bash

# PostgreSQL Migration Script for Films Application
# This script automates the migration process from SQL Server to PostgreSQL

set -e  # Exit on error

echo "=========================================="
echo "Films Application - PostgreSQL Migration"
echo "=========================================="
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configuration
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
INFRASTRUCTURE_PROJECT="$PROJECT_ROOT/src/Films.Infrastructure"
WEB_PROJECT="$PROJECT_ROOT/src/Films.Web"
MIGRATIONS_DIR="$INFRASTRUCTURE_PROJECT/Migrations"

# PostgreSQL connection details (update these)
PG_HOST="${PG_HOST:-localhost}"
PG_PORT="${PG_PORT:-5432}"
PG_DATABASE="${PG_DATABASE:-films}"
PG_USER="${PG_USER:-postgres}"
PG_PASSWORD="${PG_PASSWORD:-postgres}"

echo "Configuration:"
echo "  Project Root: $PROJECT_ROOT"
echo "  PostgreSQL Host: $PG_HOST"
echo "  PostgreSQL Port: $PG_PORT"
echo "  PostgreSQL Database: $PG_DATABASE"
echo "  PostgreSQL User: $PG_USER"
echo ""

# Function to print colored messages
print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠ $1${NC}"
}

print_info() {
    echo -e "ℹ $1"
}

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null; then
    print_error "dotnet CLI is not installed. Please install .NET 8.0 SDK."
    exit 1
fi
print_success "dotnet CLI found"

# Check if dotnet-ef is installed
if ! dotnet tool list -g | grep -q "dotnet-ef"; then
    print_warning "dotnet-ef tool not found. Installing..."
    dotnet tool install --global dotnet-ef
    print_success "dotnet-ef tool installed"
else
    print_success "dotnet-ef tool found"
fi

# Check if PostgreSQL client is installed
if ! command -v psql &> /dev/null; then
    print_warning "psql client is not installed. Database verification will be skipped."
    SKIP_DB_CHECK=true
else
    print_success "psql client found"
    SKIP_DB_CHECK=false
fi

# Step 1: Clean the solution
echo ""
print_info "Step 1: Cleaning the solution..."
cd "$PROJECT_ROOT"
dotnet clean > /dev/null 2>&1
print_success "Solution cleaned"

# Step 2: Restore packages
echo ""
print_info "Step 2: Restoring NuGet packages..."
dotnet restore > /dev/null 2>&1
print_success "Packages restored"

# Step 3: Build the solution
echo ""
print_info "Step 3: Building the solution..."
if dotnet build --no-restore > /dev/null 2>&1; then
    print_success "Solution built successfully"
else
    print_error "Build failed. Please check the error messages above."
    exit 1
fi

# Step 4: Remove old migrations
echo ""
print_info "Step 4: Checking for existing migrations..."
if [ -d "$MIGRATIONS_DIR" ]; then
    print_warning "Found existing migrations directory. Backing up..."
    BACKUP_DIR="$MIGRATIONS_DIR.backup.$(date +%Y%m%d_%H%M%S)"
    mv "$MIGRATIONS_DIR" "$BACKUP_DIR"
    print_success "Migrations backed up to: $BACKUP_DIR"
else
    print_info "No existing migrations found"
fi

# Step 5: Create database (if PostgreSQL client is available)
if [ "$SKIP_DB_CHECK" = false ]; then
    echo ""
    print_info "Step 5: Checking PostgreSQL database..."
    
    # Check if database exists
    export PGPASSWORD="$PG_PASSWORD"
    if psql -h "$PG_HOST" -p "$PG_PORT" -U "$PG_USER" -lqt | cut -d \| -f 1 | grep -qw "$PG_DATABASE"; then
        print_warning "Database '$PG_DATABASE' already exists"
        read -p "Do you want to drop and recreate it? (y/N): " -n 1 -r
        echo
        if [[ $REPLY =~ ^[Yy]$ ]]; then
            psql -h "$PG_HOST" -p "$PG_PORT" -U "$PG_USER" -c "DROP DATABASE IF EXISTS $PG_DATABASE;" > /dev/null 2>&1
            psql -h "$PG_HOST" -p "$PG_PORT" -U "$PG_USER" -c "CREATE DATABASE $PG_DATABASE;" > /dev/null 2>&1
            print_success "Database recreated"
        fi
    else
        print_info "Creating database '$PG_DATABASE'..."
        psql -h "$PG_HOST" -p "$PG_PORT" -U "$PG_USER" -c "CREATE DATABASE $PG_DATABASE;" > /dev/null 2>&1
        print_success "Database created"
    fi
    unset PGPASSWORD
else
    echo ""
    print_warning "Step 5: Skipping database creation (psql not available)"
    print_info "Please ensure the PostgreSQL database '$PG_DATABASE' exists"
fi

# Step 6: Create new migrations
echo ""
print_info "Step 6: Creating new PostgreSQL migrations..."
cd "$WEB_PROJECT"
if dotnet ef migrations add InitialCreate --project "$INFRASTRUCTURE_PROJECT" --startup-project "$WEB_PROJECT" > /dev/null 2>&1; then
    print_success "Migrations created successfully"
else
    print_error "Failed to create migrations. Please check the error messages."
    exit 1
fi

# Step 7: Apply migrations
echo ""
print_info "Step 7: Applying migrations to database..."
if dotnet ef database update --project "$INFRASTRUCTURE_PROJECT" --startup-project "$WEB_PROJECT"; then
    print_success "Migrations applied successfully"
else
    print_error "Failed to apply migrations. Please check the error messages."
    exit 1
fi

# Step 8: Verify database schema
if [ "$SKIP_DB_CHECK" = false ]; then
    echo ""
    print_info "Step 8: Verifying database schema..."
    export PGPASSWORD="$PG_PASSWORD"
    
    echo ""
    echo "Tables in database:"
    psql -h "$PG_HOST" -p "$PG_PORT" -U "$PG_USER" -d "$PG_DATABASE" -c "\dt public.*"
    
    echo ""
    echo "Migration history:"
    psql -h "$PG_HOST" -p "$PG_PORT" -U "$PG_USER" -d "$PG_DATABASE" -c "SELECT * FROM public.__ef_migrations_history;"
    
    unset PGPASSWORD
    print_success "Database schema verified"
else
    echo ""
    print_warning "Step 8: Skipping database verification (psql not available)"
fi

# Step 9: Run tests (optional)
echo ""
read -p "Do you want to run tests? (y/N): " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    print_info "Running tests..."
    cd "$PROJECT_ROOT"
    if dotnet test --no-build; then
        print_success "All tests passed"
    else
        print_warning "Some tests failed. Please review the results."
    fi
fi

# Summary
echo ""
echo "=========================================="
echo "Migration Summary"
echo "=========================================="
print_success "PostgreSQL migration completed successfully!"
echo ""
echo "Next steps:"
echo "  1. Update connection string in appsettings.json with your credentials"
echo "  2. Review the migration files in: $MIGRATIONS_DIR"
echo "  3. Test the application: cd $WEB_PROJECT && dotnet run"
echo "  4. Review the migration guide: $PROJECT_ROOT/POSTGRESQL_MIGRATION.md"
echo ""
echo "Connection string format:"
echo "  Host=$PG_HOST;Port=$PG_PORT;Database=$PG_DATABASE;Username=$PG_USER;Password=YOUR_PASSWORD"
echo ""
print_success "Migration script completed!"
