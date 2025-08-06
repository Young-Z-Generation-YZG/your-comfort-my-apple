# Catalog Service Database Schema

## Overview
The Catalog service uses **MongoDB** as its primary database, implementing a document-based storage strategy optimized for flexible product data management and high-performance read/write operations.

## Database Configuration

### MongoDB Settings
```csharp
public class MongoDbSettings
{
    public const string SettingKey = "MongoDbSettings";
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}
```

### Connection String
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CatalogDb"
  }
}
```

## Collections

### 1. **IphoneModels Collection**
**Purpose**: Main product catalog for iPhone 16 models

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "name": "iPhone 16 Pro",
  "models": [
    {
      "model_name": "iPhone 16 Pro",
      "model_order": 1
    },
    {
      "model_name": "iPhone 16 Pro Max",
      "model_order": 2
    }
  ],
  "colors": [
    {
      "color_name": "Natural Titanium",
      "color_hex": "#8E8E93",
      "color_Image": "natural-titanium.jpg",
      "color_order": 1
    },
    {
      "color_name": "Blue Titanium",
      "color_hex": "#4A90E2",
      "color_Image": "blue-titanium.jpg",
      "color_order": 2
    }
  ],
  "storages": [
    {
      "capacity": "128GB",
      "price": 0
    },
    {
      "capacity": "256GB",
      "price": 100
    },
    {
      "capacity": "512GB",
      "price": 300
    }
  ],
  "general_model": "iphone-16-pro",
  "description": "The most advanced iPhone ever with A18 Pro chip...",
  "overall_sold": 1250,
  "average_rating": {
    "rating_average_value": 4.5,
    "rating_count": 1250
  },
  "rating_stars": [
    {"star": 1, "count": 10},
    {"star": 2, "count": 25},
    {"star": 3, "count": 100},
    {"star": 4, "count": 500},
    {"star": 5, "count": 615}
  ],
  "description_images": [
    {
      "image_id": "img_001",
      "image_url": "https://cloudinary.com/iphone16-pro-1.jpg",
      "image_name": "iPhone 16 Pro Front",
      "image_description": "Front view of iPhone 16 Pro",
      "image_w": 1920,
      "image_h": 1080,
      "image_bytes": 245760,
      "image_order": 1
    }
  ],
  "slug": "iphone-16-pro",
  "category_id": ObjectId("..."),
  "created_at": ISODate("2024-01-01T00:00:00Z"),
  "updated_at": ISODate("2024-01-01T00:00:00Z"),
  "is_deleted": false,
  "deleted_at": null,
  "deleted_by": null
}
```

#### Key Features
- **Flexible Product Structure**: Nested arrays for models, colors, storages
- **Rating System**: Average rating with star distribution
- **Image Management**: Cloudinary integration with metadata
- **SEO Optimization**: Slug-based URLs
- **Audit Trail**: Creation and update timestamps

### 2. **Categories Collection**
**Purpose**: Product categorization and hierarchy

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "name": "Smartphones",
  "description": "Latest smartphones and mobile devices",
  "slug": "smartphones",
  "order": 1,
  "parent_id": null,
  "created_at": ISODate("2024-01-01T00:00:00Z"),
  "updated_at": ISODate("2024-01-01T00:00:00Z"),
  "is_deleted": false,
  "deleted_at": null,
  "deleted_by": null
}
```

#### Hierarchy Support
```json
{
  "_id": ObjectId("..."),
  "name": "iPhone",
  "description": "Apple iPhone devices",
  "slug": "iphone",
  "order": 1,
  "parent_id": ObjectId("smartphones_category_id"),
  "created_at": ISODate("2024-01-01T00:00:00Z"),
  "updated_at": ISODate("2024-01-01T00:00:00Z")
}
```

### 3. **Reviews Collection**
**Purpose**: Customer reviews and ratings for products

#### Document Structure
```json
{
  "_id": ObjectId("..."),
  "product_id": ObjectId("iphone16_model_id"),
  "model_id": ObjectId("iphone16_model_id"),
  "content": "Excellent phone with amazing camera quality!",
  "rating": 5,
  "order_id": "ORD-2024-001",
  "order_item_id": "ITEM-2024-001",
  "customer_id": "user_123",
  "customer_username": "john_doe",
  "created_at": ISODate("2024-01-15T10:30:00Z"),
  "updated_at": ISODate("2024-01-15T10:30:00Z")
}
```

#### Validation Rules
- **Rating Range**: 1-5 stars
- **Required Fields**: Product ID, customer info, content, rating
- **Order Verification**: Reviews linked to actual orders

## Value Objects

### 1. **AverageRating**
```csharp
public class AverageRating : ValueObject
{
    [BsonElement("rating_average_value")]
    public decimal RatingAverageValue { get; set; }

    [BsonElement("rating_count")]
    public int RatingCount { get; set; }

    // Methods for rating calculations
    public void AddNewRating(int rating)
    public void UpdateRating(int oldRating, int newRating)
    public void RemoveRating(int rating)
}
```

### 2. **RatingStar**
```csharp
public class RatingStar : ValueObject
{
    [BsonElement("star")]
    public int Star { get; set; }

    [BsonElement("count")]
    public int Count { get; set; }
}
```

### 3. **Model**
```csharp
public class Model : ValueObject
{
    [BsonElement("model_name")]
    public string ModelName { get; set; }

    [BsonElement("model_order")]
    public int? ModelOrder { get; set; }
}
```

### 4. **Color**
```csharp
public class Color : ValueObject
{
    [BsonElement("color_name")]
    public string ColorName { get; set; }

    [BsonElement("color_hex")]
    public string ColorHex { get; set; }

    [BsonElement("color_Image")]
    public string ColorImage { get; set; }

    [BsonElement("color_order")]
    public int? ColorOrder { get; set; }
}
```

### 5. **Image**
```csharp
public class Image : ValueObject
{
    [BsonElement("image_id")]
    public string ImageId { get; private set; }

    [BsonElement("image_url")]
    public string ImageUrl { get; private set; }

    [BsonElement("image_name")]
    public string ImageName { get; private set; }

    [BsonElement("image_description")]
    public string ImageDescription { get; private set; }

    [BsonElement("image_w")]
    public decimal ImageWidth { get; private set; }

    [BsonElement("image_h")]
    public decimal ImageHeight { get; private set; }

    [BsonElement("image_bytes")]
    public decimal ImageBytes { get; private set; }

    [BsonElement("image_order")]
    public int? ImageOrder { get; private set; }
}
```

### 6. **Slug**
```csharp
public class Slug : ValueObject
{
    public string Value { get; set; } = string.Empty;

    // URL-friendly string generation
    // Removes accents, special characters
    // Converts spaces to hyphens
    // Limits to 45 characters
}
```

## Storage Enum
```csharp
public class Storage : SmartEnum<Storage>
{
    public static readonly Storage STORAGE_64 = new("64GB", 64);
    public static readonly Storage STORAGE_128 = new("128GB", 128);
    public static readonly Storage STORAGE_256 = new("256GB", 256);
    public static readonly Storage STORAGE_512 = new("512GB", 512);
    public static readonly Storage STORAGE_1024 = new("1TB", 1024);
}
```

## Data Access Patterns

### 1. **Repository Pattern**
```csharp
public interface IMongoRepository<TEntity, TId> 
    where TEntity : Entity<TId> 
    where TId : ValueObject
{
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAllAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken);
    Task<(List<TEntity> items, int totalRecords, int totalPages)> GetAllAsync(
        int? page, int? limit, FilterDefinition<TEntity>? filter, 
        SortDefinition<TEntity>? sort, CancellationToken cancellationToken);
    Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<TEntity> GetByFilterAsync(FilterDefinition<TEntity> filter, CancellationToken cancellationToken);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
```

### 2. **Pagination & Filtering**
```csharp
// Example: Get products with pagination and filtering
var filter = Builders<IPhone16Model>.Filter.Eq(x => x.CategoryId, categoryId);
var sort = Builders<IPhone16Model>.Sort.Descending(x => x.CreatedAt);

var result = await repository.GetAllAsync(
    page: 1, 
    limit: 20, 
    filter: filter, 
    sort: sort, 
    cancellationToken);
```

### 3. **Aggregation Queries**
```csharp
// Example: Get average rating by category
var pipeline = new[]
{
    new BsonDocument("$match", new BsonDocument("category_id", categoryId)),
    new BsonDocument("$group", new BsonDocument
    {
        { "_id", "$category_id" },
        { "averageRating", new BsonDocument("$avg", "$average_rating.rating_average_value") },
        { "totalProducts", new BsonDocument("$sum", 1) }
    })
};
```

## Indexing Strategy

### 1. **Primary Indexes**
```javascript
// _id index (automatic)
db.IphoneModels.createIndex({ "_id": 1 })

// Category index for filtering
db.IphoneModels.createIndex({ "category_id": 1 })

// Slug index for SEO URLs
db.IphoneModels.createIndex({ "slug": 1 })
```

### 2. **Performance Indexes**
```javascript
// Text search index
db.IphoneModels.createIndex({ 
    "name": "text", 
    "description": "text" 
})

// Compound index for sorting and filtering
db.IphoneModels.createIndex({ 
    "category_id": 1, 
    "created_at": -1 
})

// Rating index for sorting
db.IphoneModels.createIndex({ 
    "average_rating.rating_average_value": -1 
})
```

### 3. **Review Indexes**
```javascript
// Product reviews index
db.Reviews.createIndex({ "product_id": 1, "created_at": -1 })

// Customer reviews index
db.Reviews.createIndex({ "customer_id": 1 })

// Rating index for statistics
db.Reviews.createIndex({ "rating": 1 })
```

## Data Validation

### 1. **Document Validation**
```javascript
db.runCommand({
    collMod: "IphoneModels",
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["name", "models", "colors", "storages"],
            properties: {
                name: { bsonType: "string" },
                models: { bsonType: "array" },
                colors: { bsonType: "array" },
                storages: { bsonType: "array" },
                rating: {
                    bsonType: "object",
                    properties: {
                        value: { bsonType: "number" },
                        count: { bsonType: "int" }
                    }
                }
            }
        }
    }
})
```

### 2. **Application-Level Validation**
```csharp
// Rating validation
if (rating < 1 || rating > 5)
    throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

// Required field validation
ArgumentException.ThrowIfNullOrWhiteSpace(content);
```

## Performance Optimization

### 1. **Query Optimization**
- **Projection**: Select only required fields
- **Index Usage**: Ensure queries use appropriate indexes
- **Aggregation Pipeline**: Use MongoDB aggregation for complex queries
- **Connection Pooling**: Optimize connection management

### 2. **Caching Strategy**
- **Redis Integration**: Cache frequently accessed products
- **Query Result Caching**: Cache expensive aggregation results
- **Image CDN**: Cloudinary for optimized image delivery

### 3. **Data Consistency**
- **Event Sourcing**: Domain events for data consistency
- **Eventual Consistency**: Acceptable for product catalog
- **Optimistic Concurrency**: Handle concurrent updates

## Migration & Schema Evolution

### 1. **Backward Compatibility**
- **Optional Fields**: New fields are optional in existing documents
- **Default Values**: Provide defaults for missing fields
- **Versioning**: Document versioning for major schema changes

### 2. **Migration Scripts**
```javascript
// Example: Add new field to existing documents
db.IphoneModels.updateMany(
    { "new_field": { $exists: false } },
    { $set: { "new_field": "default_value" } }
)
```

### 3. **Data Seeding**
```javascript
// Initialize collections
db.createCollection("IphoneModels");
db.createCollection("Categories");
db.createCollection("Reviews");

// Seed categories
db.Categories.insertMany([
    {
        _id: ObjectId(),
        name: "Smartphones",
        description: "Latest smartphones",
        slug: "smartphones",
        order: 1,
        created_at: new Date(),
        updated_at: new Date()
    }
]);
```

## Monitoring & Observability

### 1. **Database Metrics**
- **Query Performance**: Monitor slow queries
- **Index Usage**: Track index hit rates
- **Storage Growth**: Monitor collection sizes
- **Connection Pool**: Track connection utilization

### 2. **Health Checks**
```csharp
public class MongoHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _database.RunCommandAsync<BsonDocument>(
                new BsonDocument("ping", 1), 
                cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
```

## Best Practices

### 1. **Document Design**
- **Embedded Documents**: Keep related data together
- **Denormalization**: Optimize for read performance
- **Atomic Operations**: Use MongoDB's atomic operations
- **Index Strategy**: Create indexes based on query patterns

### 2. **Data Integrity**
- **Domain Validation**: Validate data at application level
- **Event Sourcing**: Maintain audit trail through events
- **Soft Deletes**: Logical deletion for data recovery
- **Consistency Patterns**: Eventual consistency for scalability

### 3. **Performance Guidelines**
- **Query Optimization**: Use appropriate indexes
- **Connection Management**: Proper connection pooling
- **Caching Strategy**: Redis for frequently accessed data
- **Monitoring**: Comprehensive performance monitoring
