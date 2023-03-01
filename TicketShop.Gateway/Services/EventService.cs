using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TicketShop.EventCatalog.Settings;
using TicketShop.Gateway.Dtos;

namespace TicketShop.Gateway.Services;

public class EventService : IEventService
{
    private readonly HttpClient _httpClient;
    private readonly IServiceRegistry _serviceRegistry;
    private readonly ApplicationSettings _appSettings;

    public EventService(HttpClient httpClient, IServiceRegistry serviceRegistry, IOptions<ApplicationSettings> appSettings)
    {
        _httpClient = httpClient;
        _serviceRegistry = serviceRegistry;
        _appSettings = appSettings.Value;
    }
    
    public async Task<IEnumerable<EventDataTransferObject>> GetEventsAsync(Guid? categoryId, string name)
    {
        var eventsUrl = await this._serviceRegistry.GetService(_appSettings.EventCatalogServiceId);
        var baseUrl = $"{eventsUrl.Origin}/events";
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
        var eventsUrl = await this._serviceRegistry.GetService(_appSettings.EventCatalogServiceId);
        var baseUrl = $"{eventsUrl.Origin}/events";
        var result = await this._httpClient.GetStringAsync($"{baseUrl}/{eventId}");
        return JsonConvert.DeserializeObject<EventDataTransferObject>(result);
    }
}