using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TicketShop.Payments.Dtos;

namespace TicketShop.Payments;

public class PaymentService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly EventingBasicConsumer _consumer;
        
    public PaymentService()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare("payment-queue", false, false, false, null);
        _consumer = new EventingBasicConsumer(_channel);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Received += async (model,content) =>
        {
            var body = content.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var paymentInformation = JsonConvert.DeserializeObject<PaymentInformationDataTransferObject>(json);
            var paymentResult = await ProcessPayment(paymentInformation, cancellationToken);
            var message = $"El pago para el carrito {paymentInformation.BasketId} se proceso con estado {paymentResult.PaymentTransaction.Status}";
            Console.WriteLine(message);
            NotifyPaymentResult(paymentResult.PaymentTransaction);
        };
        _channel.BasicConsume("payment-queue", true, _consumer);
        return Task.CompletedTask;
    }

    // private async void OnReceived(object? model, BasicDeliverEventArgs content)
    // {
    //     var body = content.Body.ToArray();
    //     var json = Encoding.UTF8.GetString(body);
    //     var paymentInformation = JsonConvert.DeserializeObject<PaymentInformationDataTransferObject>(json);
    //     var paymentResult = await ProcessPayment(paymentInformation);
    //     var message = $"El pago para el carrito {paymentInformation.BasketId} se proceso con estado {paymentResult}";
    //     Console.WriteLine(message);
    // }

    private void NotifyPaymentResult(PaymentTransaction paymentTransaction)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672
        };

        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("payment-results", false, false, false, null);
                var json = JsonConvert.SerializeObject(paymentTransaction);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(string.Empty, "payment-results", null, body);
            }
        }
    }
    private async Task<PaymentInformationDataTransferObject> ProcessPayment(PaymentInformationDataTransferObject paymentInformation,
        CancellationToken token)
    {
        var errors = new List<string>();
        if (paymentInformation.PaymentMethod.CardNumber == Guid.Empty)
        {
            errors.Add("La tarjeta debe tener un numero valido.");
        }

        await Task.Delay(2000, token);

        if (paymentInformation.PaymentMethod.Cvv <= 0)
        {
            errors.Add("La tarjeta debe tener un cvv valido.");
        }
        
        await Task.Delay(2000, token);

        if (paymentInformation.BasketId == Guid.Empty)
        {
            errors.Add("El pago debe ser procesado para un basket valido.");
        }

        if (paymentInformation.PaymentMethod.Month <= 0 || paymentInformation.PaymentMethod.Year < 0)
        {
            errors.Add("La tarjeta debe tener una fecha de expiracion valida.");
        }

        paymentInformation.PaymentTransaction.Status = errors.Any() ? Status.Errored : Status.Done;
        paymentInformation.PaymentTransaction.Errors = errors;
        return paymentInformation;
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