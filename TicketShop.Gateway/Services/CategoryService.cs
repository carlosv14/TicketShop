using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TicketShop.EventCatalog.Settings;
using TicketShop.Gateway.Dtos;

namespace TicketShop.Gateway.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly IServiceRegistry _serviceRegistry;
    private readonly ApplicationSettings _appSettings;

    public CategoryService(HttpClient httpClient, IServiceRegistry serviceRegistry, IOptions<ApplicationSettings> appSettings)
    {
        _httpClient = httpClient;
        _serviceRegistry = serviceRegistry;
        _appSettings = appSettings.Value;
    }
    public async Task<IEnumerable<CategoryDataTransferObject>> GetAsync()
    {
        var categoriesUrl = await this._serviceRegistry.GetService(_appSettings.EventCatalogServiceId);
        var result = await _httpClient.GetStringAsync($"{categoriesUrl.Origin}/categories");
        return JsonConvert.DeserializeObject<IEnumerable<CategoryDataTransferObject>>(result);
    }

    public async Task<CategoryDataTransferObject> GetAsync(Guid id)
    {
        var categoriesUrl = await this._serviceRegistry.GetService(_appSettings.EventCatalogServiceId);
        var result = await _httpClient.GetStringAsync($"{categoriesUrl.Origin}/categories/{id}");
        return JsonConvert.DeserializeObject<CategoryDataTransferObject>(result);
    }
}