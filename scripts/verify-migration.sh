#!/bin/bash

# PostgreSQL Migration Verification Script
# This script verifies that the migration was completed successfully

set -e

echo "=========================================="
echo "PostgreSQL Migration Verification"
echo "=========================================="
echo ""

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

# Counters
PASSED=0
FAILED=0
WARNINGS=0

# Functions
print_pass() {
    echo -e "${GREEN}✓ PASS${NC}: $1"
    ((PASSED++))
}

print_fail() {
    echo -e "${RED}✗ FAIL${NC}: $1"
    ((FAILED++))
}

print_warn() {
    echo -e "${YELLOW}⚠ WARN${NC}: $1"
    ((WARNINGS++))
}

print_info() {
    echo -e "${BLUE}ℹ INFO${NC}: $1"
}

print_section() {
    echo ""
    echo "=========================================="
    echo "$1"
    echo "=========================================="
}

# Configuration
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
INFRASTRUCTURE_PROJECT="$PROJECT_ROOT/src/Films.Infrastructure"
WEB_PROJECT="$PROJECT_ROOT/src/Films.Web"

# Test 1: Check .NET SDK
print_section "1. Checking Prerequisites"
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    print_pass ".NET SDK installed (version $DOTNET_VERSION)"
else
    print_fail ".NET SDK not found"
fi

# Test 2: Check EF Core tools
if dotnet tool list -g | grep -q "dotnet-ef"; then
    EF_VERSION=$(dotnet tool list -g | grep "dotnet-ef" | awk '{print $2}')
    print_pass "EF Core tools installed (version $EF_VERSION)"
else
    print_fail "EF Core tools not installed"
fi

# Test 3: Check PostgreSQL packages
print_section "2. Checking Package References"
if grep -q "Npgsql.EntityFrameworkCore.PostgreSQL" "$INFRASTRUCTURE_PROJECT/Films.Infrastructure.csproj"; then
    print_pass "Npgsql.EntityFrameworkCore.PostgreSQL package found"
else
    print_fail "Npgsql.EntityFrameworkCore.PostgreSQL package not found"
fi

if grep -q "EFCore.NamingConventions" "$INFRASTRUCTURE_PROJECT/Films.Infrastructure.csproj"; then
    print_pass "EFCore.NamingConventions package found"
else
    print_fail "EFCore.NamingConventions package not found"
fi

if grep -q "Microsoft.EntityFrameworkCore.SqlServer" "$INFRASTRUCTURE_PROJECT/Films.Infrastructure.csproj"; then
    print_fail "SQL Server package still present (should be removed)"
else
    print_pass "SQL Server package removed"
fi

# Test 4: Check DbContext configuration
print_section "3. Checking DbContext Configuration"
if grep -q "UseNpgsql" "$WEB_PROJECT/Program.cs"; then
    print_pass "UseNpgsql found in Program.cs"
else
    print_fail "UseNpgsql not found in Program.cs"
fi

if grep -q "UseSqlServer" "$WEB_PROJECT/Program.cs"; then
    print_fail "UseSqlServer still present in Program.cs"
else
    print_pass "UseSqlServer removed from Program.cs"
fi

if grep -q "UseSnakeCaseNamingConvention" "$WEB_PROJECT/Program.cs"; then
    print_pass "Snake case naming convention configured"
else
    print_fail "Snake case naming convention not configured"
fi

if grep -q "MigrationsHistoryTable" "$WEB_PROJECT/Program.cs"; then
    print_pass "Migrations history table configured"
else
    print_warn "Migrations history table not explicitly configured"
fi

# Test 5: Check connection strings
print_section "4. Checking Connection Strings"
if grep -q "Host=" "$WEB_PROJECT/appsettings.json"; then
    print_pass "PostgreSQL connection string format in appsettings.json"
else
    print_fail "PostgreSQL connection string not found in appsettings.json"
fi

if grep -q "Server=" "$WEB_PROJECT/appsettings.json"; then
    print_fail "SQL Server connection string still present in appsettings.json"
else
    print_pass "SQL Server connection string removed from appsettings.json"
fi

# Test 6: Check entity configurations
print_section "5. Checking Entity Configurations"
DBCONTEXT_FILE="$INFRASTRUCTURE_PROJECT/Data/FilmsDbContext.cs"

if grep -q "HasDefaultSchema(\"public\")" "$DBCONTEXT_FILE"; then
    print_pass "Default schema set to public"
else
    print_warn "Default schema not explicitly set to public"
fi

if grep -q "ToTable(\"films\")" "$DBCONTEXT_FILE"; then
    print_pass "Snake case table names found (films)"
else
    print_fail "Snake case table names not found"
fi

if grep -q "HasColumnName(\"first_name\")" "$DBCONTEXT_FILE"; then
    print_pass "Snake case column names found (first_name)"
else
    print_fail "Snake case column names not found"
fi

if grep -q "timestamp without time zone" "$DBCONTEXT_FILE"; then
    print_pass "PostgreSQL timestamp type configured"
else
    print_fail "PostgreSQL timestamp type not configured"
fi

# Test 7: Check build
print_section "6. Checking Build"
cd "$PROJECT_ROOT"
if dotnet build --no-restore > /dev/null 2>&1; then
    print_pass "Solution builds successfully"
else
    print_fail "Solution build failed"
fi

# Test 8: Check for migrations
print_section "7. Checking Migrations"
MIGRATIONS_DIR="$INFRASTRUCTURE_PROJECT/Migrations"
if [ -d "$MIGRATIONS_DIR" ]; then
    MIGRATION_COUNT=$(find "$MIGRATIONS_DIR" -name "*.cs" -type f | wc -l)
    if [ "$MIGRATION_COUNT" -gt 0 ]; then
        print_pass "Migrations directory exists with $MIGRATION_COUNT files"
    else
        print_warn "Migrations directory exists but is empty"
    fi
else
    print_warn "Migrations directory not found (run migration creation)"
fi

# Test 9: Check documentation
print_section "8. Checking Documentation"
if [ -f "$PROJECT_ROOT/POSTGRESQL_MIGRATION.md" ]; then
    print_pass "Migration guide (POSTGRESQL_MIGRATION.md) exists"
else
    print_fail "Migration guide not found"
fi

if [ -f "$PROJECT_ROOT/MIGRATION_REPORTS.json" ]; then
    print_pass "Migration reports (MIGRATION_REPORTS.json) exists"
else
    print_fail "Migration reports not found"
fi

if [ -f "$PROJECT_ROOT/QUICK_START.md" ]; then
    print_pass "Quick start guide exists"
else
    print_warn "Quick start guide not found"
fi

# Test 10: Check scripts
print_section "9. Checking Migration Scripts"
if [ -f "$PROJECT_ROOT/scripts/postgresql-setup.sql" ]; then
    print_pass "PostgreSQL setup script exists"
else
    print_warn "PostgreSQL setup script not found"
fi

if [ -f "$PROJECT_ROOT/scripts/migrate-to-postgresql.sh" ]; then
    if [ -x "$PROJECT_ROOT/scripts/migrate-to-postgresql.sh" ]; then
        print_pass "Migration script (bash) exists and is executable"
    else
        print_warn "Migration script exists but is not executable"
    fi
else
    print_warn "Migration script (bash) not found"
fi

# Test 11: Check PostgreSQL connection (optional)
print_section "10. Checking PostgreSQL Connection (Optional)"
if command -v psql &> /dev/null; then
    print_info "psql client found - attempting connection test"
    
    # Extract connection details from appsettings.json
    if [ -f "$WEB_PROJECT/appsettings.json" ]; then
        CONNECTION_STRING=$(grep -A 1 "DefaultConnection" "$WEB_PROJECT/appsettings.json" | tail -1 | sed 's/.*"\(.*\)".*/\1/')
        
        # Parse connection string (basic parsing)
        if [[ $CONNECTION_STRING == *"Host="* ]]; then
            print_info "Connection string found in appsettings.json"
            print_warn "Manual database connection test recommended"
        fi
    fi
else
    print_info "psql client not found - skipping connection test"
fi

# Summary
print_section "Verification Summary"
echo ""
echo "Results:"
echo -e "  ${GREEN}Passed${NC}:   $PASSED"
echo -e "  ${RED}Failed${NC}:   $FAILED"
echo -e "  ${YELLOW}Warnings${NC}: $WARNINGS"
echo ""

if [ $FAILED -eq 0 ]; then
    echo -e "${GREEN}=========================================="
    echo "✓ Migration verification PASSED"
    echo "==========================================${NC}"
    echo ""
    echo "Next steps:"
    echo "  1. Create database: sudo -u postgres psql -c 'CREATE DATABASE films;'"
    echo "  2. Run migrations: cd src/Films.Web && dotnet ef database update --project ../Films.Infrastructure"
    echo "  3. Test application: dotnet run"
    echo ""
    exit 0
else
    echo -e "${RED}=========================================="
    echo "✗ Migration verification FAILED"
    echo "==========================================${NC}"
    echo ""
    echo "Please review the failed checks above and fix the issues."
    echo "Refer to POSTGRESQL_MIGRATION.md for detailed guidance."
    echo ""
    exit 1
fi
