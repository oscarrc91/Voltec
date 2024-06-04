

namespace GNB_Products.Domain.Entities;
public class Rate
{
    public Rate(string from, string to, double rate)
    {
        this.from = from;
        this.to = to;
        this.rate = rate;
    }

    public string from { get; private set; }
    public string to { get; private set; }
    public double rate { get; private set; }
}