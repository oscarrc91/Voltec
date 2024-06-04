using GNB_Products.Domain.Events;
using GNB_Products.Services;
using MediatR;

namespace GNB_Products.Features.Transactions.EventHandlers;

/// <summary>
/// "Notifica" por correo avisando de la nueva transaction
/// </summary>
public class TransactionCreatedNotificationEventHandler : INotificationHandler<TransactionCreatedEvent>
{
    private readonly ILogger<TransactionCreatedNotificationEventHandler> _logger;
    private readonly IEmailSender _emailSender;

    public TransactionCreatedNotificationEventHandler(
        ILogger<TransactionCreatedNotificationEventHandler> logger,
        IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }

    public async Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Nueva notificación: Nueva transaction {Transaction}", notification.Transaction);

        await _emailSender.SendNotification("random@email.com", "Nueva Transaction", $"Transaction {notification.Transaction.sku}");
    }
}
