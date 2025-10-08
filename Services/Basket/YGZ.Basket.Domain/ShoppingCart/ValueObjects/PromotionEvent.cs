namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class PromotionEvent
{
    public required string EventId { get; init; }
    public required string EventItemId { get; init; }
    public static PromotionEvent Create(string eventId, string eventItemId)
    {
        return new PromotionEvent
        {
            EventId = eventId,
            EventItemId = eventItemId,
        };
    }
}
