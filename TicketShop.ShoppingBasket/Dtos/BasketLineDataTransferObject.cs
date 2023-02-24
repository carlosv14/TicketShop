namespace TicketShop.ShoppingBasket.Dtos;

public class BasketLineDataTransferObject
{
    public Guid BasketId { get; set; }

    public Guid BasketLineId { get; set; }

    public Guid EventId { get; set; }

    public decimal Price { get; set; }

    public int TicketQuantity { get; set; }
}