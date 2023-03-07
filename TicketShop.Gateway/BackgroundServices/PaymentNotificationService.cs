using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TicketShop.Gateway.Dtos;

namespace TicketShop.Gateway.BackgroundServices;

public class PaymentNotificationService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;

    public PaymentNotificationService()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare("payment-results", false, false, false, null);
        _consumer = new EventingBasicConsumer(_channel);
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Received += async (model,content) =>
        {
            var body = content.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var paymentInformation = JsonConvert.DeserializeObject<PaymentTransaction>(json);
            var existingTransaction = Database.PaymentTransactions.FirstOrDefault(x => x.Id == paymentInformation.Id);
            Database.PaymentTransactions.Remove(existingTransaction);
            Database.PaymentTransactions.Add(paymentInformation);
        };
        _channel.BasicConsume("payment-results", true, _consumer);
        return Task.CompletedTask;
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Servicio de pagos ejecutandose {DateTimeOffset.Now}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}