using GNB_Products.Domain.Entities;

namespace GNB_Products.Domain.Events;
public class TransactionCreatedEvent : DomainEvent
{
    public TransactionCreatedEvent(Transaction transaction)
    {
        Transaction = transaction;
    }

    public Transaction Transaction { get; }
}
