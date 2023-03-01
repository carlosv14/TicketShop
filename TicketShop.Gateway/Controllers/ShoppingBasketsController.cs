using Microsoft.AspNetCore.Mvc;
using TicketShop.Gateway.Dtos;
using TicketShop.Gateway.Services;

namespace TicketShop.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoppingBasketsController : ControllerBase
{
    private readonly IShoppingBasketService _shoppingBasketService;

    public ShoppingBasketsController(IShoppingBasketService shoppingBasketService)
    {
        _shoppingBasketService = shoppingBasketService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]CreateBasketDataTransferObject basketToCreate)
    {
        var result = await _shoppingBasketService.AddAsync(basketToCreate);
        return Ok(result);
    }

    [HttpGet("{basketId}")]
    public async Task<IActionResult> Get(Guid basketId)
    {
        var result = await _shoppingBasketService.GetAsync(basketId);
        return Ok(result);
    }
}