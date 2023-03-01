using Microsoft.AspNetCore.Mvc;
using TicketShop.EventCatalog.Dtos;
using TicketShop.EventCatalog.Services;
using TicketShop.EventCatalog.Settings;

[assembly:ApiController]
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient<IServiceRegistry, ServiceRegistry>();
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

var serviceRegistry =  app.Services.GetService<IServiceRegistry>();
await serviceRegistry.AddService(new ServiceRegistryDataTransferObject
{
    Service = "ticketshop.eventcatalog",
    Origin = "http://localhost:5085"
});

app.Run();
