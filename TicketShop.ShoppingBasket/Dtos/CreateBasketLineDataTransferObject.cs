namespace TicketShop.ShoppingBasket.Dtos;

public class CreateBasketLineDataTransferObject
{
    public Guid BasketId { get; set; }

    public Guid EventId { get; set; }

    public int TicketQuantity { get; set; }
}