

namespace GNB_Products.Domain.Entities;
public class Transaction
{
    public Transaction(string sku, double amount, string currency)
    {
        this.sku = sku;
        this.amount = amount;
        this.currency = currency;
    }

    public string sku { get; private set; }
    public double amount { get; private set; }
    public string currency { get; private set; }
}