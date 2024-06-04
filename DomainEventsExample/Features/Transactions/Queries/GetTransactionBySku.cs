using Carter;
using GNB_Products.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Diagnostics;
using static GNB_Products.Features.Transactions.Queries.GetTransactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GNB_Products.Features.Transactions.Queries;

public class GetTransactionBySku : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/transactionBySku/{sku}", (IMediator mediator, string sku) =>
        {
            return mediator.Send(new GetTransactionBySkuQuery(sku));
        })
        .WithName(nameof(GetTransactionBySku))
        .WithTags(nameof(Transaction));
    }

    public class GetTransactionBySkuQuery : IRequest<List<GetTransactionsResponse>>
    {
        public string sku;

        public GetTransactionBySkuQuery(string sku)
        {
            this.sku = sku;
        }
    }

    public class GetTransactionsHandler : IRequestHandler<GetTransactionBySkuQuery, List<GetTransactionsResponse>>
    {

        public Task<List<GetTransactionsResponse>> Handle(GetTransactionBySkuQuery request, CancellationToken cancellationToken)
        {
            string json = File.ReadAllText("JsonData/transactions.json");
            List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(json).Where(x => x.sku == request.sku).ToList();
            List<Transaction> transactionsToEur = new List<Transaction>();

            List<Rate> conversiones = JsonConvert.DeserializeObject<List<Rate>>(File.ReadAllText("JsonData/rates.json"));
            double total = 0;

            foreach (Transaction trans in transactions)
            {
                if (trans.currency != "EUR")
                {
                    double eurAmount = ConvertToEur(conversiones, trans.currency, trans.amount);
                    if (eurAmount >= 0)
                    {
                        transactionsToEur.Add(new Transaction(trans.sku, eurAmount, "EUR"));
                        total += eurAmount;
                    }
                    else // No se encontró una tasa de cambio válida
                    {
                        transactionsToEur.Add(new Transaction(trans.sku, 0, $"No se encontró una tasa de cambio válida de {trans.currency} a EUR. Importe original: {trans.amount} {trans.currency}."));
                    }
                }
                else
                {
                    transactionsToEur.Add(trans);
                    total += trans.amount;
                }
            }

            // Añadir suma total de transacciones
            if (transactions.Count > 0) {
                total = Math.Round(total, 2, MidpointRounding.ToEven); // Redondear al entero par más cercano (Banker's Rounding)
                transactionsToEur.Add(new Transaction($"TOTAL transactions from {request.sku}", total, "EUR"));
            }

            return Task.FromResult(transactionsToEur.Select(s => new GetTransactionsResponse
            (
                s.sku, s.amount, s.currency
            )).ToList());
        }
    }

    // Función para buscar la tasa de cambio entre dos monedas
    static double BuscarTasaCambio(List<Rate> conversiones, string monedaOrigen, string monedaDestino)
    {
        foreach (var conversion in conversiones)
        {
            if (conversion.from == monedaOrigen && conversion.to == monedaDestino)
            {
                return conversion.rate;
            }
            else if (conversion.from == monedaDestino && conversion.to == monedaOrigen)
            {
                return 1 / conversion.rate; // Conversión contraria
            }
        }
        return -1; // No se encontró una tasa de cambio válida
    }

    // Función para convertir moneda de manera recursiva
    static double ConvertToEur(List<Rate> conversiones, string monedaOrigen, double cantidad)
    {
        
        double tasaCambio = BuscarTasaCambio(conversiones, monedaOrigen, "EUR");
        if (tasaCambio > 0)
        {
            return Math.Round((cantidad * tasaCambio), 2, MidpointRounding.ToEven); // Redondear al entero par más cercano (Banker's Rounding)
        }
        else
        {
            // Buscar una moneda intermedia
            foreach (var conversion in conversiones)
            {
                if (conversion.from == monedaOrigen)
                {
                    double cantidadIntermedia = ConvertToEur(conversiones, conversion.to, cantidad);
                    if (cantidadIntermedia >= 0)
                    {
                        return cantidadIntermedia;
                    }
                }
            }
            return -1; // No se encuentra una tasa de cambio válida
        }
    }

    public record GetTransactionBySkuResponse(string sku, double amount, string currency);
}
