#!/bin/bash

# MongoDB Replica Set Configuration Script
# This script initializes the MongoDB replica set
# It will be mounted into the container and executed during startup

echo "Starting MongoDB replica set configuration..."

# Wait for MongoDB to be ready - simple approach
echo "Waiting for MongoDB to be ready..."
sleep 30

# Get the host IP from environment variable or use default
HOST_IP=${MONGO_HOST_IP:-"192.168.0.4"}

echo "Attempting to initialize replica set with host IP: $HOST_IP"

# Try to initialize replica set
mongosh --host db.catalog.replica.primary --port 27017 --eval "
rs.initiate({
    _id: 'rs0',
    members: [
        {_id: 0, host: '$HOST_IP:27001'},
        {_id: 1, host: '$HOST_IP:27002'},
        {_id: 2, host: '$HOST_IP:27003', arbiterOnly: true}
    ]
})
" 2>&1

echo "Replica set initialization attempted."

# Wait a moment for the replica set to stabilize
echo "Waiting for replica set to stabilize..."
sleep 10

# Display replica set status
echo "Replica set status:"
mongosh --host db.catalog.replica.primary --port 27017 --eval "rs.status().members.forEach(m => print(m.name + ': ' + m.stateStr))" 2>&1

echo "✅ MongoDB replica set configuration completed!"