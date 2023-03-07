using Microsoft.AspNetCore.Mvc;
using TicketShop.EventCatalog.Settings;
using TicketShop.Gateway.BackgroundServices;
using TicketShop.Gateway.Services;

[assembly:ApiController]

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient<IServiceRegistry, ServiceRegistry>();
builder.Services.AddControllers();
builder.Services.AddHttpClient<IEventService, EventService>();
builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddHttpClient<IShoppingBasketService, ShoppingBasketService>();
builder.Services.AddHostedService<PaymentNotificationService>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();