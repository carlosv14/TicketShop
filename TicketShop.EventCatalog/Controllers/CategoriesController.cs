using Microsoft.AspNetCore.Mvc;
using TicketShop.EventCatalog.Dtos;

namespace TicketShop.EventCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private static IEnumerable<CategoryDataTransferObject> categories = new List<CategoryDataTransferObject>
    {
        new CategoryDataTransferObject
        {
            Id = Guid.Parse("2e9899e9-8d53-42ed-968a-ef1d0453ffbc"),
            Name = "Concerts"
        },
        new CategoryDataTransferObject
        {
            Id = Guid.Parse("91a4e258-26f9-424f-831d-625d84463a41"),
            Name = "Sports"
        }
    };
    
    // categories
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(categories);
    }

    // categories/shdfhsdfhsfgs
    [HttpGet("{categoryId}")]
    public IActionResult Get(Guid categoryId)
    {
        var category = categories.SingleOrDefault(x => x.Id == categoryId);
        return category is null ? NotFound($"No se encontró la categoría con el id {category}") : Ok(category);
    }
    
}