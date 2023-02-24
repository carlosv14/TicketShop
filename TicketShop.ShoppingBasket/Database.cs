using TicketShop.ShoppingBasket.Dtos;

namespace TicketShop.ShoppingBasket;

public static class Database
{
    public static readonly List<BasketDataTransferObject> Baskets = new()
    {
        new BasketDataTransferObject
        {
            BasketId = Guid.NewGuid(),
            NumberOfItems = 0,
            UserId = Guid.Parse("89d6c721-be46-4ee9-86be-b608833e8731")
        }
    };

    public static readonly List<BasketLineDataTransferObject> BasketLines = new();

    public static readonly List<EventDataTransferObject> Events = new();
}