using TicketShop.EventCatalog.Dtos;

namespace TicketShop.EventCatalog.Services;

public interface IServiceRegistry
{
    Task<ServiceRegistryDataTransferObject> GetService(string id);
    
    Task AddService(ServiceRegistryDataTransferObject service);
}