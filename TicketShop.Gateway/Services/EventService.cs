using Newtonsoft.Json;
using TicketShop.Gateway.Dtos;

namespace TicketShop.Gateway.Services;

public class EventService : IEventService
{
    private readonly HttpClient _httpClient;
    public EventService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<EventDataTransferObject>> GetEventsAsync(Guid? categoryId, string name)
    {
        var baseUrl = "http://localhost:5069/events";
        if (categoryId is not null && name is null)
        {
            baseUrl = $"{baseUrl}?categoryId={categoryId}";
        }
        if (name is not null && categoryId is null)
        {
            baseUrl = $"{baseUrl}?name={name}";
        }

        if (name is not null && categoryId is not null)
        {
            baseUrl = $"{baseUrl}?categoryId={categoryId}&name={name}";
        }
        var result = await this._httpClient.GetStringAsync(baseUrl);
        return JsonConvert.DeserializeObject<IEnumerable<EventDataTransferObject>>(result);
    }

    public async Task<EventDataTransferObject> GetEventAsync(Guid eventId)
    {
        var baseUrl = "http://localhost:5069/events";
        var result = await this._httpClient.GetStringAsync($"{baseUrl}/{eventId}");
        return JsonConvert.DeserializeObject<EventDataTransferObject>(result);
    }
}