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
  "ProductItems": [
    {
        "id": "0000-0000-0000-0000",
        "name": "Iphone 16"
    },
    {
        "id": "0000-0000-0000-0000",
        "name": "Iphone 16 Plus"
    }
  ]
  "createdAt": "",
  "updatedAt": "",
  "categoryId": "0000-0000-0000-0000",
  "promotionId": "0000-0000-0000-0000",
  "productItemIds": ["0000-0000-0000-0000", "0000-0000-0000-0000"]
}
```
