# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /src

# Copy solution and project files for dependency caching
COPY Films.sln ./
COPY src/Films.Domain/Films.Domain.csproj ./src/Films.Domain/
COPY src/Films.Application/Films.Application.csproj ./src/Films.Application/
COPY src/Films.Infrastructure/Films.Infrastructure.csproj ./src/Films.Infrastructure/
COPY src/Films.Web/Films.Web.csproj ./src/Films.Web/
COPY tests/Films.Tests/Films.Tests.csproj ./tests/Films.Tests/

# Restore dependencies
RUN dotnet restore Films.sln

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/src/Films.Web
RUN dotnet build Films.Web.csproj -c Release --no-restore

# Publish the application
RUN dotnet publish Films.Web.csproj -c Release -o /app/publish --no-restore --no-build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application from builder stage
COPY --from=builder /app/publish .

# Set ownership to non-root user
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:80 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Expose application port
EXPOSE 80

# Configure graceful shutdown
STOPSIGNAL SIGTERM

# Start the application
ENTRYPOINT ["dotnet", "Films.Web.dll"]
