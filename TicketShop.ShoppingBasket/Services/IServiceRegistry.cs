using TicketShop.ShoppingBasket.Dtos;

namespace TicketShop.ShoppingBasket.Services;

public interface IServiceRegistry
{
    Task<ServiceRegistryDataTransferObject> GetService(string id);
    
    Task AddService(ServiceRegistryDataTransferObject service);
}