using Microsoft.AspNetCore.Mvc;
using TicketShop.Gateway.Services;

namespace TicketShop.Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _categoryService.GetAsync();
        return Ok(result);
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetAsync(Guid categoryId)
    {
        var result = await _categoryService.GetAsync(categoryId);
        return Ok(result);
    }
}