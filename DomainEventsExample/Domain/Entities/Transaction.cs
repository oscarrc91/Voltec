using GNB_Products.Domain.Events;

namespace GNB_Products.Domain.Entities;
public class Transaction : IHasDomainEvent
{
    public Transaction(string sku, double amount, string currency)
    {
        this.sku = sku;
        this.amount = amount;
        this.currency = currency;

        DomainEvents.Add(new TransactionCreatedEvent(this));
    }

    public string sku { get; private set; }
    public double amount { get; private set; }
    public string currency { get; private set; }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}