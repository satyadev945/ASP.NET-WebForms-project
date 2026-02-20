# Films.Web - AWS ECS Fargate Deployment Guide

## Table of Contents
1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Local Development Setup](#local-development-setup)
4. [Docker Deployment](#docker-deployment)
5. [AWS ECS Fargate Deployment](#aws-ecs-fargate-deployment)
6. [Configuration Management](#configuration-management)
7. [Monitoring and Logging](#monitoring-and-logging)
8. [Troubleshooting](#troubleshooting)
9. [Security Considerations](#security-considerations)

## Overview

Films.Web is an ASP.NET Core 8.0 web application designed for containerized deployment on AWS ECS Fargate. This guide provides comprehensive instructions for building, deploying, and managing the application in production environments.

### Technology Stack
- **Framework**: .NET 8.0 (ASP.NET Core)
- **Database**: SQL Server with Entity Framework Core
- **Caching**: Redis (optional)
- **Logging**: Serilog with Console sink
- **Health Checks**: ASP.NET Core Health Checks with EF Core
- **Container Platform**: Docker
- **Deployment Platform**: AWS ECS Fargate

### Application Architecture
The application follows Clean Architecture principles with the following layers:
- **Films.Domain**: Core domain entities and interfaces
- **Films.Application**: Business logic and application services
- **Films.Infrastructure**: Data access and external service implementations
- **Films.Web**: Presentation layer (ASP.NET Core MVC/Razor Pages)

## Prerequisites

### Required Tools
1. **Docker Desktop** (version 20.10 or later)
   - Download: https://www.docker.com/products/docker-desktop
   - Verify: `docker --version`

2. **AWS CLI** (version 2.x)
   - Download: https://aws.amazon.com/cli/
   - Verify: `aws --version`
   - Configure: `aws configure`

3. **.NET 8.0 SDK** (for local development)
   - Download: https://dotnet.microsoft.com/download/dotnet/8.0
   - Verify: `dotnet --version`

### AWS Account Requirements
1. **IAM User** with the following permissions:
   - ECS Full Access
   - ECR Full Access
   - CloudWatch Logs Full Access
   - IAM Role Creation (for task execution role)
   - VPC and Security Group management
   - Application Load Balancer management

2. **AWS Resources**:
   - VPC with at least 2 subnets (in different availability zones)
   - Security Group allowing inbound traffic on port 80
   - SQL Server database (RDS or external)
   - Redis instance (ElastiCache or external) - optional

### IAM Roles Required

#### ECS Task Execution Role
Create an IAM role named `ecsTaskExecutionRole` with the following policy:
```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "ecr:GetAuthorizationToken",
        "ecr:BatchCheckLayerAvailability",
        "ecr:GetDownloadUrlForLayer",
        "ecr:BatchGetImage",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": "*"
    }
  ]
}
```

#### ECS Task Role (Optional)
Create an IAM role named `ecsTaskRole` for application-specific permissions:
```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "s3:GetObject",
        "s3:PutObject",
        "secretsmanager:GetSecretValue"
      ],
      "Resource": "*"
    }
  ]
}
```

## Local Development Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd dotnetback
```

### 2. Configure Application Settings
Edit `src/Films.Web/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FilmsDb;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;MultipleActiveResultSets=true",
    "Redis": ""
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information"
      }
    }
  }
}
```

### 3. Run Database Migrations
```bash
cd src/Films.Web
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run --project src/Films.Web/Films.Web.csproj
```

Access the application at: http://localhost:5000

## Docker Deployment

### Build Docker Image Locally
```bash
docker build -t films-web:latest -f Dockerfile .
```

### Run with Docker Compose
```bash
# Set environment variables
export DB_CONNECTION_STRING="Server=host.docker.internal;Database=FilmsDb;User Id=sa;Password=YourPassword123;TrustServerCertificate=True"
export REDIS_CONNECTION_STRING=""

# Start the application
docker-compose up -d

# View logs
docker-compose logs -f

# Stop the application
docker-compose down
```

Access the application at: http://localhost:8080

### Test Health Endpoints
```bash
# Liveness check
curl http://localhost:8080/health/live

# Readiness check
curl http://localhost:8080/health/ready

# Full health check
curl http://localhost:8080/health
```

## AWS ECS Fargate Deployment

### Step 1: Build and Push Docker Image

#### Option A: Using AWS ECR
```bash
# Make script executable
chmod +x scripts/build-push.sh

# Run the script
./scripts/build-push.sh

# Follow the prompts:
# 1. Select "1" for AWS ECR
# 2. Enter AWS region (e.g., us-east-1)
# 3. Enter AWS Account ID
# 4. Enter ECR repository name (default: films-web)
# 5. Enter image tag (default: latest)
```

#### Option B: Using Docker Hub
```bash
# Run the script
./scripts/build-push.sh

# Follow the prompts:
# 1. Select "2" for Docker Hub
# 2. Enter Docker Hub username
# 3. Enter Docker Hub password/token
# 4. Enter image tag (default: latest)
```

#### Windows Users
Use `scripts/build-push.bat` instead:
```cmd
scripts\build-push.bat
```

### Step 2: Prepare AWS Infrastructure

#### Create VPC and Subnets (if not exists)
```bash
# Create VPC
aws ec2 create-vpc --cidr-block 10.0.0.0/16 --region us-east-1

# Create subnets in different availability zones
aws ec2 create-subnet --vpc-id vpc-xxxxx --cidr-block 10.0.1.0/24 --availability-zone us-east-1a
aws ec2 create-subnet --vpc-id vpc-xxxxx --cidr-block 10.0.2.0/24 --availability-zone us-east-1b
```

#### Create Security Group
```bash
# Create security group
aws ec2 create-security-group \
  --group-name films-web-sg \
  --description "Security group for Films.Web application" \
  --vpc-id vpc-xxxxx

# Allow inbound HTTP traffic
aws ec2 authorize-security-group-ingress \
  --group-id sg-xxxxx \
  --protocol tcp \
  --port 80 \
  --cidr 0.0.0.0/0

# Allow inbound HTTPS traffic (optional)
aws ec2 authorize-security-group-ingress \
  --group-id sg-xxxxx \
  --protocol tcp \
  --port 443 \
  --cidr 0.0.0.0/0
```

### Step 3: Deploy to ECS Fargate

#### Using Deployment Script
```bash
# Make script executable
chmod +x scripts/deploy-image.sh

# Run the deployment script
./scripts/deploy-image.sh
```

#### Deployment Prompts
The script will prompt for the following information:

1. **AWS Configuration**:
   - AWS region (e.g., us-east-1)
   - ECS cluster name (e.g., films-cluster)

2. **Network Configuration**:
   - VPC ID (e.g., vpc-0abc123def456)
   - Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456)
   - Security Group ID (e.g., sg-0abc123def)

3. **Container Configuration**:
   - Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/films-web:latest)

4. **Database Configuration**:
   - Database connection string
   - Redis connection string (optional)

5. **Load Balancer**:
   - Whether to create an Application Load Balancer (y/n)

#### Windows Users
Use `scripts/deploy-image.bat` instead:
```cmd
scripts\deploy-image.bat
```

### Step 4: Verify Deployment

#### Check Service Status
```bash
aws ecs describe-services \
  --cluster films-cluster \
  --services films-web-service \
  --region us-east-1
```

#### List Running Tasks
```bash
aws ecs list-tasks \
  --cluster films-cluster \
  --service-name films-web-service \
  --region us-east-1
```

#### View Task Details
```bash
aws ecs describe-tasks \
  --cluster films-cluster \
  --tasks <task-arn> \
  --region us-east-1
```

## ECS Task Definition Explained

### CPU and Memory Configuration
AWS Fargate requires specific CPU and memory combinations:

| CPU (vCPU) | Memory (MB) Options |
|------------|---------------------|
| 256 (.25)  | 512, 1024, 2048 |
| 512 (.5)   | 1024, 2048, 3072, 4096 |
| 1024 (1)   | 2048-8192 (increments of 1024) |
| 2048 (2)   | 4096-16384 (increments of 1024) |
| 4096 (4)   | 8192-30720 (increments of 1024) |

**Default Configuration**: CPU: 512, Memory: 1024

### Network Mode
- **awsvpc**: Required for Fargate
- Each task gets its own elastic network interface (ENI)
- Tasks have their own private IP addresses

### Container Definition
```json
{
  "name": "films-web",
  "image": "123456789.dkr.ecr.us-east-1.amazonaws.com/films-web:latest",
  "essential": true,
  "portMappings": [
    {
      "containerPort": 80,
      "protocol": "tcp"
    }
  ],
  "environment": [
    {
      "name": "ASPNETCORE_ENVIRONMENT",
      "value": "Production"
    }
  ],
  "logConfiguration": {
    "logDriver": "awslogs",
    "options": {
      "awslogs-group": "/ecs/films-web",
      "awslogs-region": "us-east-1",
      "awslogs-stream-prefix": "ecs"
    }
  }
}
```

## ECS Service Configuration

### Deployment Configuration
- **Desired Count**: 2 (for high availability)
- **Maximum Percent**: 200 (allows rolling updates)
- **Minimum Healthy Percent**: 50 (ensures availability during updates)

### Load Balancer Integration
When using an Application Load Balancer:
- **Target Type**: IP (required for awsvpc network mode)
- **Health Check Path**: /health
- **Health Check Interval**: 30 seconds
- **Healthy Threshold**: 2
- **Unhealthy Threshold**: 3
- **Grace Period**: 300 seconds (allows for application startup)

### Auto Scaling (Optional)
Configure service auto scaling based on CPU or memory utilization:
```bash
# Register scalable target
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --resource-id service/films-cluster/films-web-service \
  --scalable-dimension ecs:service:DesiredCount \
  --min-capacity 2 \
  --max-capacity 10

# Create scaling policy
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --resource-id service/films-cluster/films-web-service \
  --scalable-dimension ecs:service:DesiredCount \
  --policy-name cpu-scaling-policy \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration file://scaling-policy.json
```

## Configuration Management

### Environment Variables
The application uses the following environment variables:

| Variable | Description | Default |
|----------|-------------|---------|
| ASPNETCORE_ENVIRONMENT | Application environment | Production |
| ASPNETCORE_URLS | Kestrel listening URLs | http://+:80 |
| ConnectionStrings__DefaultConnection | SQL Server connection string | Required |
| ConnectionStrings__Redis | Redis connection string | Optional |
| DOTNET_RUNNING_IN_CONTAINER | Container detection flag | true |

### Secrets Management
For production deployments, use AWS Secrets Manager:

1. **Create Secret**:
```bash
aws secretsmanager create-secret \
  --name films-web/db-connection \
  --secret-string "Server=xxx;Database=FilmsDb;User Id=xxx;Password=xxx"
```

2. **Update Task Definition**:
```json
{
  "secrets": [
    {
      "name": "ConnectionStrings__DefaultConnection",
      "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:films-web/db-connection"
    }
  ]
}
```

3. **Update Task Execution Role**:
Add permission to read secrets:
```json
{
  "Effect": "Allow",
  "Action": [
    "secretsmanager:GetSecretValue"
  ],
  "Resource": "arn:aws:secretsmanager:us-east-1:123456789:secret:films-web/*"
}
```

## Monitoring and Logging

### CloudWatch Logs
View application logs:
```bash
# Tail logs in real-time
aws logs tail /ecs/films-web --follow --region us-east-1

# Filter logs by pattern
aws logs filter-log-events \
  --log-group-name /ecs/films-web \
  --filter-pattern "ERROR" \
  --region us-east-1

# Get logs for specific time range
aws logs filter-log-events \
  --log-group-name /ecs/films-web \
  --start-time 1609459200000 \
  --end-time 1609545600000 \
  --region us-east-1
```

### CloudWatch Metrics
Monitor ECS service metrics:
- CPUUtilization
- MemoryUtilization
- TargetResponseTime (with ALB)
- RequestCount (with ALB)
- HealthyHostCount (with ALB)

### Application Insights (Optional)
For advanced monitoring, integrate Application Insights:

1. **Add NuGet Package**:
```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

2. **Configure in Program.cs**:
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

3. **Set Connection String**:
```bash
export APPLICATIONINSIGHTS_CONNECTION_STRING="InstrumentationKey=xxx"
```

## Troubleshooting

### Common Issues

#### 1. Task Fails to Start
**Symptoms**: Tasks transition from PENDING to STOPPED immediately

**Possible Causes**:
- Invalid Docker image URI
- Insufficient IAM permissions
- Invalid CPU/memory combination
- Container health check failures

**Solutions**:
```bash
# Check task stopped reason
aws ecs describe-tasks \
  --cluster films-cluster \
  --tasks <task-arn> \
  --region us-east-1 \
  --query 'tasks[0].stoppedReason'

# Check CloudWatch logs
aws logs tail /ecs/films-web --follow --region us-east-1

# Verify IAM role permissions
aws iam get-role --role-name ecsTaskExecutionRole
```

#### 2. Database Connection Failures
**Symptoms**: Application logs show database connection errors

**Solutions**:
- Verify database connection string
- Check security group allows traffic from ECS tasks
- Ensure database is accessible from VPC subnets
- Verify database credentials

```bash
# Test database connectivity from ECS task
aws ecs execute-command \
  --cluster films-cluster \
  --task <task-id> \
  --container films-web \
  --interactive \
  --command "/bin/bash"
```

#### 3. Load Balancer Health Check Failures
**Symptoms**: Tasks are marked unhealthy by ALB

**Solutions**:
- Verify health check endpoint returns 200 OK
- Increase health check grace period
- Check application startup time
- Verify security group allows ALB to reach tasks

```bash
# Test health endpoint
curl http://<alb-dns>/health

# Check target group health
aws elbv2 describe-target-health \
  --target-group-arn <target-group-arn> \
  --region us-east-1
```

#### 4. Out of Memory Errors
**Symptoms**: Tasks are killed due to memory exhaustion

**Solutions**:
- Increase task memory allocation
- Optimize application memory usage
- Check for memory leaks
- Monitor memory metrics in CloudWatch

```bash
# Update service with more memory
aws ecs update-service \
  --cluster films-cluster \
  --service films-web-service \
  --task-definition films-web-task:2 \
  --region us-east-1
```

#### 5. Deployment Stuck
**Symptoms**: Service deployment doesn't complete

**Solutions**:
- Check if old tasks are draining properly
- Verify new tasks are passing health checks
- Review deployment circuit breaker status
- Check CloudWatch logs for errors

```bash
# Force new deployment
aws ecs update-service \
  --cluster films-cluster \
  --service films-web-service \
  --force-new-deployment \
  --region us-east-1
```

### Debug Commands

#### View Service Events
```bash
aws ecs describe-services \
  --cluster films-cluster \
  --services films-web-service \
  --region us-east-1 \
  --query 'services[0].events[0:10]'
```

#### Get Task Logs
```bash
# Get task ID
TASK_ID=$(aws ecs list-tasks \
  --cluster films-cluster \
  --service-name films-web-service \
  --region us-east-1 \
  --query 'taskArns[0]' \
  --output text)

# View logs
aws logs get-log-events \
  --log-group-name /ecs/films-web \
  --log-stream-name ecs/films-web/$TASK_ID \
  --region us-east-1
```

#### Check Container Status
```bash
aws ecs describe-tasks \
  --cluster films-cluster \
  --tasks <task-arn> \
  --region us-east-1 \
  --query 'tasks[0].containers[0].{name:name,status:lastStatus,exitCode:exitCode,reason:reason}'
```

## Security Considerations

### 1. Network Security
- Use private subnets for ECS tasks when possible
- Restrict security group rules to minimum required access
- Use VPC endpoints for AWS services (ECR, CloudWatch, Secrets Manager)
- Enable VPC Flow Logs for network monitoring

### 2. Container Security
- Use official Microsoft base images
- Run containers as non-root user
- Scan images for vulnerabilities (AWS ECR scanning)
- Keep base images and dependencies updated
- Use read-only root filesystem when possible

### 3. Secrets Management
- Never hardcode secrets in Docker images
- Use AWS Secrets Manager or Parameter Store
- Rotate secrets regularly
- Use IAM roles for AWS service authentication
- Enable encryption at rest for secrets

### 4. Application Security
- Enable HTTPS/TLS for all external communication
- Implement proper authentication and authorization
- Use security headers (HSTS, CSP, X-Frame-Options)
- Enable CORS only for trusted origins
- Implement rate limiting and request throttling

### 5. Monitoring and Auditing
- Enable CloudTrail for API audit logging
- Set up CloudWatch alarms for security events
- Monitor failed authentication attempts
- Review security group changes
- Enable AWS Config for compliance monitoring

### 6. IAM Best Practices
- Follow principle of least privilege
- Use separate IAM roles for different environments
- Enable MFA for IAM users
- Regularly review and rotate access keys
- Use IAM policies to restrict resource access

## Performance Optimization

### 1. .NET Runtime Optimization
```dockerfile
# Enable ReadyToRun compilation
RUN dotnet publish -c Release -o /app/publish \
    --no-restore --no-build \
    -p:PublishReadyToRun=true
```

### 2. Container Optimization
- Use multi-stage builds to reduce image size
- Leverage Docker layer caching
- Minimize number of layers
- Use .dockerignore to exclude unnecessary files

### 3. Application Optimization
- Enable response compression
- Use distributed caching (Redis)
- Implement database connection pooling
- Use async/await for I/O operations
- Enable HTTP/2 support

### 4. ECS Optimization
- Use appropriate task size (CPU/memory)
- Enable container insights for detailed metrics
- Use spot instances for cost savings (non-production)
- Implement proper health checks
- Configure auto scaling based on metrics

## Blue/Green Deployments

For zero-downtime deployments, use ECS blue/green deployment with CodeDeploy:

1. **Create CodeDeploy Application**:
```bash
aws deploy create-application \
  --application-name films-web-app \
  --compute-platform ECS
```

2. **Create Deployment Group**:
```bash
aws deploy create-deployment-group \
  --application-name films-web-app \
  --deployment-group-name films-web-dg \
  --service-role-arn arn:aws:iam::123456789:role/CodeDeployServiceRole \
  --ecs-services clusterName=films-cluster,serviceName=films-web-service \
  --load-balancer-info targetGroupPairInfoList=[...]
```

3. **Deploy New Version**:
```bash
aws deploy create-deployment \
  --application-name films-web-app \
  --deployment-group-name films-web-dg \
  --revision revisionType=AppSpecContent,appSpecContent={content=...}
```

## Rollback Procedures

### Manual Rollback
```bash
# List task definition revisions
aws ecs list-task-definitions \
  --family-prefix films-web-task \
  --region us-east-1

# Update service to previous revision
aws ecs update-service \
  --cluster films-cluster \
  --service films-web-service \
  --task-definition films-web-task:1 \
  --region us-east-1
```

### Automatic Rollback
Enable deployment circuit breaker in service definition:
```json
{
  "deploymentConfiguration": {
    "deploymentCircuitBreaker": {
      "enable": true,
      "rollback": true
    }
  }
}
```

## Cost Optimization

### 1. Right-Sizing
- Monitor CPU and memory utilization
- Adjust task size based on actual usage
- Use smaller task sizes for non-production environments

### 2. Spot Instances
For non-critical workloads, use Fargate Spot:
```json
{
  "capacityProviderStrategy": [
    {
      "capacityProvider": "FARGATE_SPOT",
      "weight": 1
    }
  ]
}
```

### 3. Auto Scaling
- Scale down during off-peak hours
- Use target tracking scaling policies
- Set appropriate minimum and maximum task counts

### 4. Resource Cleanup
- Delete unused ECR images
- Remove old task definition revisions
- Clean up CloudWatch log streams
- Delete unused load balancers and target groups

## Additional Resources

### Documentation
- [AWS ECS Documentation](https://docs.aws.amazon.com/ecs/)
- [AWS Fargate Documentation](https://docs.aws.amazon.com/fargate/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Docker Documentation](https://docs.docker.com/)

### Tools
- [AWS Copilot CLI](https://aws.github.io/copilot-cli/) - Simplified ECS deployment
- [ECS CLI](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/ECS_CLI.html) - Command-line tool for ECS
- [AWS CloudFormation](https://aws.amazon.com/cloudformation/) - Infrastructure as Code

### Support
- AWS Support: https://aws.amazon.com/support/
- .NET Community: https://dotnet.microsoft.com/platform/community
- Stack Overflow: https://stackoverflow.com/questions/tagged/aws-ecs

## Conclusion

This deployment guide provides comprehensive instructions for deploying the Films.Web application to AWS ECS Fargate. Follow the security best practices, monitor your application regularly, and optimize based on actual usage patterns.

For questions or issues, please refer to the troubleshooting section or contact your DevOps team.
