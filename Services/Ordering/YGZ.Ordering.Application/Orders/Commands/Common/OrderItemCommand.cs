namespace YGZ.Ordering.Application.Orders.Commands.Common;

#pragma warning disable CS8618

public sealed record OrderItemCommand
{
    public string ProductId { get; set; }

    public string ProductModel { get; set; }

    public string ProductColor { get; set; }

    public string ProductColorHex { get; set; }

    public int ProductStorage { get; set; }

    public decimal ProductUnitPrice { get; set; }

    public string ProductImage { get; set; }

    public int Quantity { get; set; }
}