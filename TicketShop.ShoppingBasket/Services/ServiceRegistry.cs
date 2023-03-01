using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TicketShop.ShoppingBasket.Dtos;
using TicketShop.ShoppingBasket.Settings;

namespace TicketShop.ShoppingBasket.Services;

public class ServiceRegistry : IServiceRegistry
{
    private readonly HttpClient _httpClient;
    private readonly ApplicationSettings _applicationSettings;

    public ServiceRegistry(HttpClient httpClient, IOptions<ApplicationSettings> applicationSettings)
    {
        _httpClient = httpClient;
        _applicationSettings = applicationSettings.Value;
    }
    public async Task<ServiceRegistryDataTransferObject> GetService(string id)
    {
        var result = await this._httpClient.GetStringAsync($"{_applicationSettings.ServiceRegistryUrl}/services/{id}");
        return JsonConvert.DeserializeObject<ServiceRegistryDataTransferObject>(result);
    }

    public async Task AddService(ServiceRegistryDataTransferObject service)
    {
        var result = await this._httpClient
            .PostAsJsonAsync(
                $"{_applicationSettings.ServiceRegistryUrl}/services",
                service);
        result.EnsureSuccessStatusCode();
    }
}