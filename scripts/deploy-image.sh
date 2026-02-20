#!/bin/bash
set -e
set -o pipefail

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}   Films.Web ECS Fargate Deployment${NC}"
echo -e "${BLUE}========================================${NC}"
echo ""

# Prompt for deployment configuration
echo -e "${YELLOW}AWS Configuration${NC}"
read -p "Enter AWS region (e.g., us-east-1): " AWS_REGION
read -p "Enter ECS cluster name (e.g., films-cluster): " CLUSTER_NAME

echo ""
echo -e "${YELLOW}Network Configuration${NC}"
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNET_IDS
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP

# Parse subnet IDs
IFS=',' read -ra SUBNETS <<< "$SUBNET_IDS"
SUBNET_1="${SUBNETS[0]}"
SUBNET_2="${SUBNETS[1]:-$SUBNET_1}"

echo ""
echo -e "${YELLOW}Container Configuration${NC}"
read -p "Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/films-web:latest): " IMAGE_URI

echo ""
echo -e "${YELLOW}Database Configuration${NC}"
read -p "Enter database connection string: " DB_CONNECTION_STRING
read -p "Enter Redis connection string (optional, press Enter to skip): " REDIS_CONNECTION_STRING

echo ""
echo -e "${YELLOW}Load Balancer Configuration${NC}"
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB

# Get AWS Account ID
echo ""
echo -e "${YELLOW}Retrieving AWS Account ID...${NC}"
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo -e "${GREEN}Account ID: $ACCOUNT_ID${NC}"

# Check if ECS cluster exists, create if not
echo ""
echo -e "${YELLOW}Checking ECS cluster...${NC}"
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1 || {
    echo -e "${YELLOW}Creating ECS cluster: $CLUSTER_NAME${NC}"
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
    echo -e "${GREEN}ECS cluster created successfully${NC}"
}

# Create CloudWatch log group
echo ""
echo -e "${YELLOW}Creating CloudWatch log group...${NC}"
aws logs create-log-group --log-group-name "/ecs/films-web" --region "$AWS_REGION" 2>/dev/null || echo -e "${BLUE}Log group already exists${NC}"

# Handle load balancer configuration
if [[ "$NEED_LB" == "y" || "$NEED_LB" == "Y" ]]; then
    echo ""
    echo -e "${YELLOW}Creating Application Load Balancer...${NC}"
    
    # Create ALB
    ALB_ARN=$(aws elbv2 create-load-balancer \
        --name films-web-alb \
        --subnets "$SUBNET_1" "$SUBNET_2" \
        --security-groups "$SECURITY_GROUP" \
        --scheme internet-facing \
        --type application \
        --ip-address-type ipv4 \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text 2>/dev/null || aws elbv2 describe-load-balancers \
        --names films-web-alb \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text)
    
    echo -e "${GREEN}Load Balancer ARN: $ALB_ARN${NC}"
    
    # Create Target Group with target-type ip (required for Fargate)
    echo -e "${YELLOW}Creating Target Group...${NC}"
    TARGET_GROUP_ARN=$(aws elbv2 create-target-group \
        --name films-web-tg \
        --protocol HTTP \
        --port 80 \
        --vpc-id "$VPC_ID" \
        --target-type ip \
        --health-check-enabled \
        --health-check-path /health \
        --health-check-interval-seconds 30 \
        --health-check-timeout-seconds 5 \
        --healthy-threshold-count 2 \
        --unhealthy-threshold-count 3 \
        --region "$AWS_REGION" \
        --query 'TargetGroups[0].TargetGroupArn' \
        --output text 2>/dev/null || aws elbv2 describe-target-groups \
        --names films-web-tg \
        --region "$AWS_REGION" \
        --query 'TargetGroups[0].TargetGroupArn' \
        --output text)
    
    echo -e "${GREEN}Target Group ARN: $TARGET_GROUP_ARN${NC}"
    
    # Create listener
    echo -e "${YELLOW}Creating ALB Listener...${NC}"
    aws elbv2 create-listener \
        --load-balancer-arn "$ALB_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" 2>/dev/null || echo -e "${BLUE}Listener already exists${NC}"
    
    # Get ALB DNS name
    ALB_DNS=$(aws elbv2 describe-load-balancers \
        --load-balancer-arns "$ALB_ARN" \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].DNSName' \
        --output text)
    
    echo -e "${GREEN}Load Balancer DNS: $ALB_DNS${NC}"
else
    echo -e "${YELLOW}Skipping load balancer creation${NC}"
    # Remove loadBalancers section from service definition
    TARGET_GROUP_ARN=""
fi

# Prepare task definition
echo ""
echo -e "${YELLOW}Preparing task definition...${NC}"
cp ecs/task-definition.json ecs/task-definition-temp.json

# Replace placeholders in task definition
sed -i "s|{{ACCOUNT_ID}}|$ACCOUNT_ID|g" ecs/task-definition-temp.json
sed -i "s|{{AWS_REGION}}|$AWS_REGION|g" ecs/task-definition-temp.json
sed -i "s|{{IMAGE_URI}}|$IMAGE_URI|g" ecs/task-definition-temp.json
sed -i "s|{{DB_CONNECTION_STRING}}|$DB_CONNECTION_STRING|g" ecs/task-definition-temp.json
sed -i "s|{{REDIS_CONNECTION_STRING}}|$REDIS_CONNECTION_STRING|g" ecs/task-definition-temp.json

# Register task definition
echo -e "${YELLOW}Registering task definition...${NC}"
TASK_DEF_ARN=$(aws ecs register-task-definition \
    --cli-input-json file://ecs/task-definition-temp.json \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

echo -e "${GREEN}Task Definition ARN: $TASK_DEF_ARN${NC}"

# Clean up temporary file
rm -f ecs/task-definition-temp.json

# Prepare service definition
echo ""
echo -e "${YELLOW}Preparing service definition...${NC}"
cp ecs/service-definition.json ecs/service-definition-temp.json

# Replace placeholders in service definition
sed -i "s|{{CLUSTER_NAME}}|$CLUSTER_NAME|g" ecs/service-definition-temp.json
sed -i "s|{{SUBNET_1}}|$SUBNET_1|g" ecs/service-definition-temp.json
sed -i "s|{{SUBNET_2}}|$SUBNET_2|g" ecs/service-definition-temp.json
sed -i "s|{{SECURITY_GROUP}}|$SECURITY_GROUP|g" ecs/service-definition-temp.json

if [[ -n "$TARGET_GROUP_ARN" ]]; then
    sed -i "s|{{TARGET_GROUP_ARN}}|$TARGET_GROUP_ARN|g" ecs/service-definition-temp.json
else
    # Remove loadBalancers section if no load balancer
    sed -i '/"loadBalancers":/,/],/d' ecs/service-definition-temp.json
    sed -i '/"healthCheckGracePeriodSeconds":/d' ecs/service-definition-temp.json
fi

# Check if service exists
echo ""
echo -e "${YELLOW}Checking if service exists...${NC}"
SERVICE_EXISTS=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services films-web-service \
    --region "$AWS_REGION" \
    --query 'services[0].serviceName' \
    --output text 2>/dev/null)

if [[ "$SERVICE_EXISTS" == "films-web-service" ]]; then
    echo -e "${YELLOW}Service exists. Updating service...${NC}"
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service films-web-service \
        --task-definition "$TASK_DEF_ARN" \
        --region "$AWS_REGION" \
        --force-new-deployment
    echo -e "${GREEN}Service updated successfully${NC}"
else
    echo -e "${YELLOW}Creating new service...${NC}"
    aws ecs create-service \
        --cli-input-json file://ecs/service-definition-temp.json \
        --region "$AWS_REGION"
    echo -e "${GREEN}Service created successfully${NC}"
fi

# Clean up temporary file
rm -f ecs/service-definition-temp.json

# Wait for service to stabilize
echo ""
echo -e "${YELLOW}Waiting for service to stabilize (this may take a few minutes)...${NC}"
aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services films-web-service \
    --region "$AWS_REGION"

echo -e "${GREEN}Service is stable${NC}"

# Verify deployment
echo ""
echo -e "${YELLOW}Verifying deployment...${NC}"
RUNNING_COUNT=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services films-web-service \
    --region "$AWS_REGION" \
    --query 'services[0].runningCount' \
    --output text)

echo -e "${GREEN}Running tasks: $RUNNING_COUNT${NC}"

echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}   Deployment Completed Successfully!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo -e "${BLUE}Deployment Details:${NC}"
echo -e "  Cluster: $CLUSTER_NAME"
echo -e "  Service: films-web-service"
echo -e "  Task Definition: $TASK_DEF_ARN"
echo -e "  Running Tasks: $RUNNING_COUNT"
echo -e "  CloudWatch Logs: /ecs/films-web"
if [[ -n "$ALB_DNS" ]]; then
    echo -e "  Load Balancer: http://$ALB_DNS"
fi
echo ""
echo -e "${YELLOW}Troubleshooting:${NC}"
echo -e "  - View logs: aws logs tail /ecs/films-web --follow --region $AWS_REGION"
echo -e "  - Check service: aws ecs describe-services --cluster $CLUSTER_NAME --services films-web-service --region $AWS_REGION"
echo -e "  - List tasks: aws ecs list-tasks --cluster $CLUSTER_NAME --service-name films-web-service --region $AWS_REGION"
echo ""
