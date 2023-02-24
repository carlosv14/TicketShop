namespace TicketShop.EventCatalog.Dtos;

public class EventDataTransferObject
{
    public Guid EventId { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public DateTime Date { get; set; }

    public string Artist { get; set; }

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; }
}