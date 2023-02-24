using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TicketShop.ShoppingBasket.Dtos;

namespace TicketShop.ShoppingBasket;

[ApiController]
[Route("[controller]")]
public class ShoppingBasketsController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] CreateBasketDataTransferObject basket)
    {
        if (Database.Baskets.Any(x => x.UserId == basket.UserId))
        {
            return BadRequest("Ya existe un basket para este usuario");
        }
        var basketDto = new BasketDataTransferObject
        {
            UserId = basket.UserId,
            BasketId = Guid.NewGuid(),
            NumberOfItems = 0
        };
        Database.Baskets.Add(basketDto);

        return Ok(basketDto);
    }

    [HttpGet("{basketId}")]
    public IActionResult Get(Guid basketId)
    {
        var basket = Database.Baskets.SingleOrDefault(x => x.BasketId == basketId);
        return basket is null ? NotFound($"No se encontró un carrito de compras con el id {basket}") : Ok(basket);
    }
}