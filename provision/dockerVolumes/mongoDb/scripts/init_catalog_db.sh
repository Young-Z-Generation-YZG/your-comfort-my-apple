#!/bin/bash

# MongoDB Catalog Database Initialization Script
# This script creates the CatalogDb database
# It will be executed after the replica set is configured

echo "Starting CatalogDb database initialization..."

# Wait for MongoDB replica set to be ready
echo "Waiting for MongoDB replica set to be ready..."
sleep 10

# Get the host IP from environment variable or use default
HOST_IP=${MONGO_HOST_IP:-"192.168.0.4"}

echo "Connecting to MongoDB replica set..."

# Create the CatalogDb database
echo "Creating CatalogDb database..."
mongosh --host db.catalog.replica.primary --port 27017 --eval "
use CatalogDb;
db.runCommand({ping: 1});
print('CatalogDb database created successfully');
" 2>&1

# Create Test collection to make database visible
echo "Creating Test collection..."
mongosh --host db.catalog.replica.primary --port 27017 --eval "
use CatalogDb;
db.createCollection('Test');
print('Test collection created successfully');
" 2>&1

# Wait a moment for collection creation to complete
sleep 2

if [ $? -eq 0 ]; then
    echo "✅ CatalogDb database initialized successfully!"
    
    # Display database info
    echo "Database information:"
    mongosh --host db.catalog.replica.primary --port 27017 --eval "
    use CatalogDb;
    print('Database name: ' + db.getName());
    print('Collections:');
    db.getCollectionNames().forEach(function(name) { print('  - ' + name); });
    print('Database stats:');
    printjson(db.stats());
    " 2>&1
    
    echo "✅ CatalogDb database initialization completed!"
else
    echo "❌ Failed to initialize CatalogDb database!"
    exit 1
fi
