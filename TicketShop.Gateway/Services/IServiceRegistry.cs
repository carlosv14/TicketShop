using TicketShop.Gateway.Dtos;

namespace TicketShop.Gateway.Services;

public interface IServiceRegistry
{
    Task<ServiceRegistryDataTransferObject> GetService(string id);
    
    Task AddService(ServiceRegistryDataTransferObject service);
}