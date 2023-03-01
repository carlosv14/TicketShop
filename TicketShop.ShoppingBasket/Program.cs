using Microsoft.AspNetCore.Mvc;
using TicketShop.ShoppingBasket.Dtos;
using TicketShop.ShoppingBasket.Services;
using TicketShop.ShoppingBasket.Settings;

[assembly:ApiController]

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient<IServiceRegistry, ServiceRegistry>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

var serviceRegistry =  app.Services.GetService<IServiceRegistry>();
await serviceRegistry.AddService(new ServiceRegistryDataTransferObject
{
    Service = "ticketshop.shoppingbasket",
    Origin = "http://localhost:5031"
});

app.Run();