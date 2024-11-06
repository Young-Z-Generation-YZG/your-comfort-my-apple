# Domain Models

## Product

```csharp
class Product
{
    Product Create();
    void AddProductItem(ProductItem productItem);
    void RemoveProductItem(ProductItem productItem);
    void UpdateProductItem(ProductItem productItem);
}
```

```json
{
  "id": "6729c960ad3ee43c7966b405",
  "name": "Iphone 16",
  "description": "desc",
  "images": [
    {
      "url": "string",
      "id": "id"
    },
  ]
  "average_rating": {
    "value": 4.5,
    "number": 1,
  },
  "Product_items": [
    {
      "id": "6729c960ad3ee43c7966b405",
      "SKU": "#STRING",
      "model": "Iphone 16",
      "color": "pink",
      "storage": 256,
      "price": 1000,
      "quantity_in_stock": 1,
      "images": [
      {
        "url": "string",
        "id": "id"
      }],
    "created_at": "2024-11-05T07:29:36.8766573Z",
    "updated_at": "2024-11-05T07:29:36.8766573Z"
    },
  ],
  "created_at": "2024-11-05T07:29:36.8766573Z",
  "updated_at": "2024-11-05T07:29:36.8766573Z",
  "category_id": "6729c960ad3ee43c7966b405",
  "promotion_id": "6729c960ad3ee43c7966b405",
}
```
