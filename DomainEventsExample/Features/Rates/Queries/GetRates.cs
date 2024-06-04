using Carter;
using GNB_Products.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GNB_Products.Features.Rates.Queries;

public class GetRates : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/rates", (IMediator mediator) =>
        {
            return mediator.Send(new GetRatesQuery());
        })
        .WithName(nameof(GetRates))
        .WithTags(nameof(Rate));
    }

    public class GetRatesQuery : IRequest<List<GetRatesResponse>>
    {

    }

    public class GetRatesHandler : IRequestHandler<GetRatesQuery, List<GetRatesResponse>>
    {

        public Task<List<GetRatesResponse>> Handle(GetRatesQuery request, CancellationToken cancellationToken)
        {
            string json = File.ReadAllText("JsonData/rates.json");
            List<Rate> rates = JsonConvert.DeserializeObject<List<Rate>>(json);

            return Task.FromResult(rates.Select(s => new GetRatesResponse
            (
                s.from, s.to, s.rate
            )).ToList());
        }
    }

    public record GetRatesResponse(string from, string to, double rate);
}
