using Microsoft.AspNetCore.Mvc;
using TicketShop.Gateway.Services;

[assembly:ApiController]

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpClient<IEventService, EventService>();
builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddHttpClient<IShoppingBasketService, ShoppingBasketService>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();