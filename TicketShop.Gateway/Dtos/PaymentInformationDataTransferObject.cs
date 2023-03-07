namespace TicketShop.Gateway.Dtos;

public class PaymentInformationDataTransferObject
{
    public PaymentMethodInformationDataTransferObject PaymentMethod { get; set; }

    public decimal Total { get; set; }

    public Guid BasketId { get; set; }

    public PaymentTransaction PaymentTransaction { get; set; }
}