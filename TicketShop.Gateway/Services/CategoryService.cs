using Newtonsoft.Json;
using TicketShop.Gateway.Dtos;

namespace TicketShop.Gateway.Services;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;

    public CategoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IEnumerable<CategoryDataTransferObject>> GetAsync()
    {
        var result = await _httpClient.GetStringAsync("http://localhost:5069/categories");
        return JsonConvert.DeserializeObject<IEnumerable<CategoryDataTransferObject>>(result);
    }

    public async Task<CategoryDataTransferObject> GetAsync(Guid id)
    {
        var result = await _httpClient.GetStringAsync($"http://localhost:5069/categories/{id}");
        return JsonConvert.DeserializeObject<CategoryDataTransferObject>(result);
    }
}