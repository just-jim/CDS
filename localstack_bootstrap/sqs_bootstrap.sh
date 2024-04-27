#!/usr/bin/env bash

set -euo pipefail

echo "configuring sqs"
echo "==================="
LOCALSTACK_HOST=localhost.localstack.cloud
AWS_REGION=us-east-1

create_queue() {
    local QUEUE_NAME=$1
    awslocal --endpoint-url=http://${LOCALSTACK_HOST}:4566 sqs create-queue --queue-name ${QUEUE_NAME} --region ${AWS_REGION} --attributes VisibilityTimeout=30
}

create_queue "assets"