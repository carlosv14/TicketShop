using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketShop.ShoppingBasket.Dtos;

namespace TicketShop.ShoppingBasket;

[ApiController]
[Route("[controller]")]
public class ShoppingBasketLinesController : ControllerBase
{
    [HttpGet("/ShoppingBaskets/{basketId}/BasketLines")]
    public IActionResult Get(Guid basketId)
    {
        var basket = Database.Baskets.SingleOrDefault(x => x.BasketId == basketId);
        if (basket is null)
        {
            return NotFound($"No se encontró el basket con id {basketId}");
        }

        var basketLines = Database.BasketLines.Where(x => x.BasketId == basketId);
        return Ok(basketLines);
    }

    [HttpPost("/ShoppingBaskets/{basketId}/BasketLines")]
    public async Task<IActionResult> Post(Guid basketId, [FromBody] CreateBasketLineDataTransferObject basketLineToCreate)
    {
        var basket = Database.Baskets.SingleOrDefault(x => x.BasketId == basketId);
        if (basket is null)
        {
            return NotFound($"No se encontró el basket con id {basketId}");
        }

        var eventData = Database.Events.SingleOrDefault(x => x.EventId == basketLineToCreate.EventId);
        if (eventData is null)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient
                    .GetStringAsync($"http://localhost:5069/events/{basketLineToCreate.EventId}");
                eventData = JsonConvert.DeserializeObject<EventDataTransferObject>(response);
                Database.Events.Add(eventData);
            } 
        }

        if (eventData is null)
        {
            return NotFound($"No se encontró un evento con el id: {basketLineToCreate.EventId}");
        }
        
        var line = new BasketLineDataTransferObject
        {
            BasketId = basketId,
            BasketLineId = Guid.NewGuid(),
            EventId = basketLineToCreate.EventId,
            TicketQuantity = basketLineToCreate.TicketQuantity,
            Price = eventData.Price * basketLineToCreate.TicketQuantity
        };
        
        Database.BasketLines.Add(line);
        return Ok(line);
    }
}