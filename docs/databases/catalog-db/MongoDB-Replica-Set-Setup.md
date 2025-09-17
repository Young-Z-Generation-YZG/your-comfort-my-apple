# MongoDB Replica Set Setup Guide

## Overview

This guide explains how to convert the standalone MongoDB instance to a replica set configuration to enable transaction support in the Catalog service.

## Why Replica Set?

MongoDB standalone instances **do not support transactions**. To use MongoDB transactions in production, you must use a replica set or sharded cluster. This setup provides:

-   **ACID Transactions**: Full transaction support for data consistency
-   **High Availability**: Automatic failover and redundancy
-   **Read Scaling**: Distribute read operations across secondary nodes
-   **Data Durability**: Multiple copies of data for protection

## Architecture

Our replica set configuration includes:

-   **Primary Node** (`mongodb:27017`): Handles all write operations
-   **Secondary Node** (`mongodb-secondary:27017`): Replicates data, handles read operations
-   **Arbiter Node** (`mongodb-arbiter:27017`): Participates in elections but doesn't store data

## Configuration Details

### Docker Compose Services

```yaml
db.catalog:
    container_name: mongodb
    command: ["mongod", "--replSet", "rs0", "--bind_ip_all", "--port", "27017"]
    ports:
        - "37017:27017"

db.catalog.secondary:
    container_name: mongodb-secondary
    command: ["mongod", "--replSet", "rs0", "--bind_ip_all", "--port", "27017"]
    ports:
        - "37018:27017"

db.catalog.arbiter:
    container_name: mongodb-arbiter
    command: ["mongod", "--replSet", "rs0", "--bind_ip_all", "--port", "27017"]
    ports:
        - "37019:27017"
```

### Connection String

```json
{
    "MongoDbSettings": {
        "ConnectionString": "mongodb://bale:bale@db.catalog:27017,db.catalog.secondary:27017,db.catalog.arbiter:27017/CatalogDb?authSource=admin&replicaSet=rs0&readPreference=primaryPreferred",
        "DatabaseName": "CatalogDb"
    }
}
```

**Connection String Parameters:**

-   `replicaSet=rs0`: Specifies the replica set name
-   `readPreference=primaryPreferred`: Prefers primary for reads, falls back to secondary
-   `authSource=admin`: Authentication database

## Deployment Steps

### 1. Stop Current Services

```bash
docker-compose down
```

### 2. Clean Up Old Volumes (Optional)

If you want to start fresh:

```bash
docker volume rm tlcn_admin_mongodb_catalog
```

### 3. Start the Replica Set

```bash
docker-compose up -d db.catalog db.catalog.secondary db.catalog.arbiter
```

### 4. Wait for Initialization

The replica set initialization happens automatically through the `replica-set-init.js` script. Monitor the logs:

```bash
docker logs mongodb
docker logs mongodb-secondary
docker logs mongodb-arbiter
```

### 5. Verify Replica Set Status

Connect to the primary MongoDB instance:

```bash
docker exec -it mongodb mongosh --username bale --password bale --authenticationDatabase admin
```

Check replica set status:

```javascript
rs.status();
```

Expected output should show all three members with one PRIMARY and one SECONDARY.

### 6. Start Application Services

```bash
docker-compose up -d ygz.catalog.api
```

## Transaction Usage in .NET

Now you can use MongoDB transactions in your Catalog service:

```csharp
public async Task<Result> CreateProductWithReviewsAsync(
    Product product,
    List<Review> reviews,
    CancellationToken cancellationToken = default)
{
    using var session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);

    try
    {
        session.StartTransaction();

        // Insert product
        await _productRepository.AddAsync(product, cancellationToken);

        // Insert reviews
        foreach (var review in reviews)
        {
            await _reviewRepository.AddAsync(review, cancellationToken);
        }

        await session.CommitTransactionAsync(cancellationToken);
        return Result.Success();
    }
    catch (Exception ex)
    {
        await session.AbortTransactionAsync(cancellationToken);
        return Result.Failure(DomainErrors.General.ServerError);
    }
}
```

## Monitoring and Maintenance

### Health Checks

The primary MongoDB instance includes a health check that verifies replica set status:

```yaml
healthcheck:
    test:
        [
            "CMD",
            "mongosh",
            "--host",
            "localhost",
            "--username",
            "bale",
            "--password",
            "bale",
            "--eval",
            "rs.status().ok",
        ]
    interval: 10s
    timeout: 10s
    retries: 5
```

### Replica Set Status Commands

```javascript
// Check replica set status
rs.status();

// Check replica set configuration
rs.conf();

// Check replica set members
rs.isMaster();

// Force reconfiguration (if needed)
rs.reconfig(newConfig);
```

### Backup Strategy

For production deployments, implement regular backups:

```bash
# Backup from primary
mongodump --host localhost:37017 --username bale --password bale --authenticationDatabase admin --db CatalogDb --out /backup

# Restore to replica set
mongorestore --host localhost:37017 --username bale --password bale --authenticationDatabase admin --db CatalogDb /backup/CatalogDb
```

## Troubleshooting

### Common Issues

1. **Replica Set Not Initialized**

    ```bash
    # Check if replica set is initialized
    docker exec -it mongodb mongosh --username bale --password bale --authenticationDatabase admin --eval "rs.status()"
    ```

2. **Connection Refused**

    - Ensure all three MongoDB containers are running
    - Check network connectivity between containers
    - Verify firewall settings

3. **Authentication Failures**

    - Verify credentials in connection string
    - Check `authSource=admin` parameter
    - Ensure user exists in admin database

4. **Transaction Failures**
    - Ensure you're using replica set connection string
    - Check replica set health: `rs.status()`
    - Verify write concern settings

### Logs and Debugging

```bash
# Check MongoDB logs
docker logs mongodb
docker logs mongodb-secondary
docker logs mongodb-arbiter

# Connect to MongoDB shell
docker exec -it mongodb mongosh --username bale --password bale --authenticationDatabase admin
```

## Performance Considerations

### Read Preference Options

-   `primary`: All reads from primary (default for transactions)
-   `primaryPreferred`: Primary preferred, fallback to secondary
-   `secondary`: All reads from secondary
-   `secondaryPreferred`: Secondary preferred, fallback to primary
-   `nearest`: Read from nearest member

### Write Concern

For critical operations, use appropriate write concern:

```csharp
var writeConcern = new WriteConcern(WriteConcern.WMajority, wTimeout: TimeSpan.FromSeconds(30));
await collection.InsertOneAsync(document, new InsertOneOptions { WriteConcern = writeConcern });
```

## Security Considerations

### Production Deployment

1. **Enable Authentication**: Use strong passwords and proper user roles
2. **Network Security**: Restrict network access to MongoDB instances
3. **SSL/TLS**: Enable encryption in transit
4. **Firewall**: Block unnecessary ports
5. **Regular Updates**: Keep MongoDB version updated

### Keyfile Authentication (Recommended for Production)

```yaml
# Add to MongoDB command
--keyFile /etc/mongodb/keyfile
```

## Migration from Standalone

If you have existing data in a standalone MongoDB instance:

1. **Backup existing data**
2. **Stop the application**
3. **Update configuration to replica set**
4. **Start replica set with initialization scripts**
5. **Restore data to primary node**
6. **Verify data replication to secondary**
7. **Start application with new connection string**

## References

-   [MongoDB Replica Set Documentation](https://www.mongodb.com/docs/manual/replication/)
-   [MongoDB Transactions Guide](https://www.mongodb.com/docs/manual/core/transactions/)
-   [Convert Standalone to Replica Set](https://www.mongodb.com/docs/manual/tutorial/convert-standalone-to-replica-set/)
