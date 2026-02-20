@echo off
setlocal enabledelayedexpansion

echo ========================================
echo    Films.Web ECS Fargate Deployment
echo ========================================
echo.

REM Prompt for deployment configuration
echo AWS Configuration
set /p AWS_REGION="Enter AWS region (e.g., us-east-1): "
set /p CLUSTER_NAME="Enter ECS cluster name (e.g., films-cluster): "

echo.
echo Network Configuration
set /p VPC_ID="Enter VPC ID (e.g., vpc-0abc123def456): "
set /p SUBNET_IDS="Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): "
set /p SECURITY_GROUP="Enter Security Group ID (e.g., sg-0abc123def): "

REM Parse subnet IDs
for /f "tokens=1,2 delims=," %%a in ("!SUBNET_IDS!") do (
    set SUBNET_1=%%a
    set SUBNET_2=%%b
)
if "!SUBNET_2!"=="" set SUBNET_2=!SUBNET_1!

echo.
echo Container Configuration
set /p IMAGE_URI="Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/films-web:latest): "

echo.
echo Database Configuration
set /p DB_CONNECTION_STRING="Enter database connection string: "
set /p REDIS_CONNECTION_STRING="Enter Redis connection string (optional, press Enter to skip): "

echo.
echo Load Balancer Configuration
set /p NEED_LB="Do you need a load balancer for this service? (y/n): "

REM Get AWS Account ID
echo.
echo Retrieving AWS Account ID...
for /f "tokens=*" %%i in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%i
echo Account ID: !ACCOUNT_ID!

REM Check if ECS cluster exists, create if not
echo.
echo Checking ECS cluster...
aws ecs describe-clusters --clusters "!CLUSTER_NAME!" --region "!AWS_REGION!" >nul 2>&1
if !ERRORLEVEL! neq 0 (
    echo Creating ECS cluster: !CLUSTER_NAME!
    aws ecs create-cluster --cluster-name "!CLUSTER_NAME!" --region "!AWS_REGION!"
    echo ECS cluster created successfully
)

REM Create CloudWatch log group
echo.
echo Creating CloudWatch log group...
aws logs create-log-group --log-group-name "/ecs/films-web" --region "!AWS_REGION!" 2>nul
if !ERRORLEVEL! neq 0 (
    echo Log group already exists
)

REM Handle load balancer configuration
if /i "!NEED_LB!"=="y" (
    echo.
    echo Creating Application Load Balancer...
    
    REM Create ALB
    for /f "tokens=*" %%i in ('aws elbv2 create-load-balancer --name films-web-alb --subnets "!SUBNET_1!" "!SUBNET_2!" --security-groups "!SECURITY_GROUP!" --scheme internet-facing --type application --ip-address-type ipv4 --region "!AWS_REGION!" --query "LoadBalancers[0].LoadBalancerArn" --output text 2^>nul') do set ALB_ARN=%%i
    
    if "!ALB_ARN!"=="" (
        for /f "tokens=*" %%i in ('aws elbv2 describe-load-balancers --names films-web-alb --region "!AWS_REGION!" --query "LoadBalancers[0].LoadBalancerArn" --output text') do set ALB_ARN=%%i
    )
    
    echo Load Balancer ARN: !ALB_ARN!
    
    REM Create Target Group with target-type ip
    echo Creating Target Group...
    for /f "tokens=*" %%i in ('aws elbv2 create-target-group --name films-web-tg --protocol HTTP --port 80 --vpc-id "!VPC_ID!" --target-type ip --health-check-enabled --health-check-path /health --health-check-interval-seconds 30 --health-check-timeout-seconds 5 --healthy-threshold-count 2 --unhealthy-threshold-count 3 --region "!AWS_REGION!" --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%i
    
    if "!TARGET_GROUP_ARN!"=="" (
        for /f "tokens=*" %%i in ('aws elbv2 describe-target-groups --names films-web-tg --region "!AWS_REGION!" --query "TargetGroups[0].TargetGroupArn" --output text') do set TARGET_GROUP_ARN=%%i
    )
    
    echo Target Group ARN: !TARGET_GROUP_ARN!
    
    REM Create listener
    echo Creating ALB Listener...
    aws elbv2 create-listener --load-balancer-arn "!ALB_ARN!" --protocol HTTP --port 80 --default-actions Type=forward,TargetGroupArn="!TARGET_GROUP_ARN!" --region "!AWS_REGION!" 2>nul
    
    REM Get ALB DNS name
    for /f "tokens=*" %%i in ('aws elbv2 describe-load-balancers --load-balancer-arns "!ALB_ARN!" --region "!AWS_REGION!" --query "LoadBalancers[0].DNSName" --output text') do set ALB_DNS=%%i
    
    echo Load Balancer DNS: !ALB_DNS!
) else (
    echo Skipping load balancer creation
    set TARGET_GROUP_ARN=
)

REM Prepare task definition
echo.
echo Preparing task definition...
copy ecs\task-definition.json ecs\task-definition-temp.json >nul

REM Replace placeholders in task definition
powershell -Command "(Get-Content ecs\task-definition-temp.json) -replace '{{ACCOUNT_ID}}', '!ACCOUNT_ID!' | Set-Content ecs\task-definition-temp.json"
powershell -Command "(Get-Content ecs\task-definition-temp.json) -replace '{{AWS_REGION}}', '!AWS_REGION!' | Set-Content ecs\task-definition-temp.json"
powershell -Command "(Get-Content ecs\task-definition-temp.json) -replace '{{IMAGE_URI}}', '!IMAGE_URI!' | Set-Content ecs\task-definition-temp.json"
powershell -Command "(Get-Content ecs\task-definition-temp.json) -replace '{{DB_CONNECTION_STRING}}', '!DB_CONNECTION_STRING!' | Set-Content ecs\task-definition-temp.json"
powershell -Command "(Get-Content ecs\task-definition-temp.json) -replace '{{REDIS_CONNECTION_STRING}}', '!REDIS_CONNECTION_STRING!' | Set-Content ecs\task-definition-temp.json"

REM Register task definition
echo Registering task definition...
for /f "tokens=*" %%i in ('aws ecs register-task-definition --cli-input-json file://ecs/task-definition-temp.json --region "!AWS_REGION!" --query "taskDefinition.taskDefinitionArn" --output text') do set TASK_DEF_ARN=%%i

echo Task Definition ARN: !TASK_DEF_ARN!

REM Clean up temporary file
del ecs\task-definition-temp.json

REM Prepare service definition
echo.
echo Preparing service definition...
copy ecs\service-definition.json ecs\service-definition-temp.json >nul

REM Replace placeholders in service definition
powershell -Command "(Get-Content ecs\service-definition-temp.json) -replace '{{CLUSTER_NAME}}', '!CLUSTER_NAME!' | Set-Content ecs\service-definition-temp.json"
powershell -Command "(Get-Content ecs\service-definition-temp.json) -replace '{{SUBNET_1}}', '!SUBNET_1!' | Set-Content ecs\service-definition-temp.json"
powershell -Command "(Get-Content ecs\service-definition-temp.json) -replace '{{SUBNET_2}}', '!SUBNET_2!' | Set-Content ecs\service-definition-temp.json"
powershell -Command "(Get-Content ecs\service-definition-temp.json) -replace '{{SECURITY_GROUP}}', '!SECURITY_GROUP!' | Set-Content ecs\service-definition-temp.json"

if not "!TARGET_GROUP_ARN!"=="" (
    powershell -Command "(Get-Content ecs\service-definition-temp.json) -replace '{{TARGET_GROUP_ARN}}', '!TARGET_GROUP_ARN!' | Set-Content ecs\service-definition-temp.json"
) else (
    REM Remove loadBalancers section if no load balancer
    powershell -Command "$content = Get-Content ecs\service-definition-temp.json -Raw; $content = $content -replace '(?s)\"loadBalancers\":\s*\[.*?\],\s*', ''; $content = $content -replace '\"healthCheckGracePeriodSeconds\":\s*\d+,\s*', ''; $content | Set-Content ecs\service-definition-temp.json"
)

REM Check if service exists
echo.
echo Checking if service exists...
for /f "tokens=*" %%i in ('aws ecs describe-services --cluster "!CLUSTER_NAME!" --services films-web-service --region "!AWS_REGION!" --query "services[0].serviceName" --output text 2^>nul') do set SERVICE_EXISTS=%%i

if "!SERVICE_EXISTS!"=="films-web-service" (
    echo Service exists. Updating service...
    aws ecs update-service --cluster "!CLUSTER_NAME!" --service films-web-service --task-definition "!TASK_DEF_ARN!" --region "!AWS_REGION!" --force-new-deployment
    echo Service updated successfully
) else (
    echo Creating new service...
    aws ecs create-service --cli-input-json file://ecs/service-definition-temp.json --region "!AWS_REGION!"
    echo Service created successfully
)

REM Clean up temporary file
del ecs\service-definition-temp.json

REM Wait for service to stabilize
echo.
echo Waiting for service to stabilize (this may take a few minutes)...
aws ecs wait services-stable --cluster "!CLUSTER_NAME!" --services films-web-service --region "!AWS_REGION!"

echo Service is stable

REM Verify deployment
echo.
echo Verifying deployment...
for /f "tokens=*" %%i in ('aws ecs describe-services --cluster "!CLUSTER_NAME!" --services films-web-service --region "!AWS_REGION!" --query "services[0].runningCount" --output text') do set RUNNING_COUNT=%%i

echo Running tasks: !RUNNING_COUNT!

echo.
echo ========================================
echo    Deployment Completed Successfully!
echo ========================================
echo.
echo Deployment Details:
echo   Cluster: !CLUSTER_NAME!
echo   Service: films-web-service
echo   Task Definition: !TASK_DEF_ARN!
echo   Running Tasks: !RUNNING_COUNT!
echo   CloudWatch Logs: /ecs/films-web
if not "!ALB_DNS!"=="" (
    echo   Load Balancer: http://!ALB_DNS!
)
echo.
echo Troubleshooting:
echo   - View logs: aws logs tail /ecs/films-web --follow --region !AWS_REGION!
echo   - Check service: aws ecs describe-services --cluster !CLUSTER_NAME! --services films-web-service --region !AWS_REGION!
echo   - List tasks: aws ecs list-tasks --cluster !CLUSTER_NAME! --service-name films-web-service --region !AWS_REGION!
echo.

endlocal
