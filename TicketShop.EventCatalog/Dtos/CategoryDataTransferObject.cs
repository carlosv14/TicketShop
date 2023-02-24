namespace TicketShop.EventCatalog.Dtos;

public class CategoryDataTransferObject
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ICollection<EventDataTransferObject> Events { get; set; }
}