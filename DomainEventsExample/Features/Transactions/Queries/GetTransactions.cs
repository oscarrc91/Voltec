using Carter;
using GNB_Products.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GNB_Products.Features.Transactions.Queries;

public class GetTransactions : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/transactions", (IMediator mediator) =>
        {
            return mediator.Send(new GetTransactionsQuery());
        })
        .WithName(nameof(GetTransactions))
        .WithTags(nameof(Transaction));
    }

    public class GetTransactionsQuery : IRequest<List<GetTransactionsResponse>>
    {

    }

    public class GetTransactionsHandler : IRequestHandler<GetTransactionsQuery, List<GetTransactionsResponse>>
    {

        public Task<List<GetTransactionsResponse>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            string json = File.ReadAllText("JsonData/transactions.json");
            List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(json);

            return Task.FromResult(transactions.Select(s => new GetTransactionsResponse
            (
                s.sku, s.amount, s.currency
            )).ToList());
        }
    }

    public record GetTransactionsResponse(string sku, double amount, string currency);
}
