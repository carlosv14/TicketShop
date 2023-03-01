using TicketShop.Gateway.Dtos;

namespace TicketShop.Gateway.Services;

public interface IShoppingBasketService
{
    Task<BasketLineDataTransferObject> AddBasketLineAsync(Guid basketId, CreateBasketLineDataTransferObject basketLine);

    Task<IEnumerable<BasketLineDataTransferObject>> GetBasketLinesAsync(Guid basketId);

    Task<BasketDataTransferObject> AddAsync(CreateBasketDataTransferObject basketToCreate);

    Task<BasketDataTransferObject> GetAsync(Guid id);
}