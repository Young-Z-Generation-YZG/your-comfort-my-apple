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
  "id": "0000-0000-0000-0000",
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
      "id": "0000-0000-0000-0000",
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
    }
  "Images": [
    {
      "url": "",
      "id": "",
    }
  ],
  ],
  "createdAt": "",
  "updatedAt": "",
  "category_id": "0000-0000-0000-0000",
  "promotion_id": "0000-0000-0000-0000",
}
```
