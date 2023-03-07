using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
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

    [HttpPost("{basketId}/pay")]
    public async Task<ActionResult> Pay(Guid basketId, [FromBody] PaymentMethodInformationDataTransferObject paymentMethodInformation)
    {
        var result = await _shoppingBasketService.GetBasketLinesAsync(basketId);
        var paymentInformation = new PaymentInformationDataTransferObject
        {
            BasketId = basketId,
            Total = result.Sum(x => x.Price),
            PaymentMethod = paymentMethodInformation,
        };

        Database.PaymentTransactions.Add(new PaymentTransaction
        {
            Id = Guid.NewGuid(),
            Status = Status.InProgress,
            Errors = new List<string>()
        });
        
        paymentInformation.PaymentTransaction = Database.PaymentTransactions.Last();
        try
        {
            var json = JsonConvert.SerializeObject(paymentInformation);
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("payment-queue", false, false, false, null);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish(string.Empty, "payment-queue", null,  body);
                }
            }

           
            return Ok(Database.PaymentTransactions.Last().Id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("/payments/{transactionId}")]
    public ActionResult<PaymentTransaction> GetTransaction(Guid transactionId)
    {
        return Ok(Database.PaymentTransactions.SingleOrDefault(x => x.Id == transactionId));
    }
}