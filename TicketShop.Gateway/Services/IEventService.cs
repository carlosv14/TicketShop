using TicketShop.Gateway.Dtos;

namespace TicketShop.Gateway.Services;

public interface IEventService
{
    Task<IEnumerable<EventDataTransferObject>> GetEventsAsync(Guid? categoryId, string? name);

    Task<EventDataTransferObject> GetEventAsync(Guid eventId);
}