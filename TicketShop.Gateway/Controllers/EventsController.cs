using Microsoft.AspNetCore.Mvc;
using TicketShop.Gateway.Services;

namespace TicketShop.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetEvents([FromQuery] Guid? categoryId, [FromQuery] string? name)
    {
        var results = await _eventService.GetEventsAsync(categoryId, name);
        return Ok(results);
    }

    [HttpGet("{eventId}")]
    public async Task<IActionResult> GetEvent(Guid eventId)
    {
        var results = await _eventService.GetEventAsync(eventId);
        return Ok(results);
    }
}