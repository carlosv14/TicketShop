using Microsoft.AspNetCore.Mvc;
using TicketShop.EventCatalog.Dtos;

namespace TicketShop.EventCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private static IEnumerable<EventDataTransferObject> events = new List<EventDataTransferObject>
    {
        new EventDataTransferObject
        {
            EventId = Guid.NewGuid(),
            Date = DateTime.Now,
            Name = "Beyonce in concert",
            Artist = "Beyonce",
            Price = 300,
            CategoryId = Guid.Parse("2e9899e9-8d53-42ed-968a-ef1d0453ffbc")
        },
        new EventDataTransferObject
        {
            EventId = Guid.NewGuid(),
            Date = DateTime.Now.AddMonths(1),
            Name = "Ferxo in concert",
            Artist = "Ferxo",
            Price = 250,
            CategoryId = Guid.Parse("2e9899e9-8d53-42ed-968a-ef1d0453ffbc")
        },
        new EventDataTransferObject
        {
            EventId = Guid.NewGuid(),
            Date = DateTime.Now.AddMonths(2),
            Name = "El tongo in concert",
            Artist = "El tongo",
            Price = 400,
            CategoryId = Guid.Parse("2e9899e9-8d53-42ed-968a-ef1d0453ffbc")
        },
        new EventDataTransferObject
        {
            EventId = Guid.NewGuid(),
            Date = DateTime.Now.AddMonths(4),
            Name = "Coldplay in concert",
            Artist = "Coldplay",
            Price = 500,
            CategoryId = Guid.Parse("2e9899e9-8d53-42ed-968a-ef1d0453ffbc")
        },
        new EventDataTransferObject
        {
            EventId = Guid.NewGuid(),
            Date = DateTime.Now.AddMonths(6),
            Name = "Olimpia vs Marathon",
            Artist = "None",
            Price = 500,
            CategoryId = Guid.Parse("91a4e258-26f9-424f-831d-625d84463a41")
        }
    };
    
    [HttpGet]
    public IActionResult Get([FromQuery] Guid? categoryId, [FromQuery] string? name)
    {
        if (categoryId is null && name is null)
        {
            return Ok(events);
        }

        var currentEvents = new List<EventDataTransferObject>();
        if (categoryId is not null)
        {
            currentEvents = events.Where(@event => @event.CategoryId == categoryId).ToList();
        }

        if (name is not null)
        {
            currentEvents = currentEvents.Any()
                ? currentEvents.Where(@event => @event.Name.Contains(name)).ToList()
                : events.Where(@event => @event.Name.Contains(name)).ToList();
        }
        return !currentEvents.Any() ? NotFound($"No se encontró ningun evento") : Ok(currentEvents);
    }

    [HttpGet("{eventId}")]
    public IActionResult Get(Guid eventId)
    {
        var @event = events.SingleOrDefault(@event => @event.EventId == eventId);
        return @event is null ? NotFound($"No se encontró un evento con el id {eventId}") : Ok(@event);
    }
}