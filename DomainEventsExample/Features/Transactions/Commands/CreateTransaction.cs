using Carter;
using Carter.ModelBinding;
using GNB_Products.Domain.Entities;
using GNB_Products.Persistence;
using FluentValidation;
using MediatR;

namespace DomainEventsExample.Features.Transactions.Commands;
public class CreateTransaction : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/transactions", async (IMediator mediator, CreateTransactionCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateTransaction))
        .WithTags(nameof(Transaction))
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status201Created);
    }

    public class CreateTransactionCommand : IRequest<IResult>
    {
        public string sku { get; set; } = string.Empty;
        public double amount { get; set; } 
        public string currency { get; set; } = string.Empty;
    }

    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, IResult>
    {
        private readonly MyDbContext _context;
        private readonly IValidator<CreateTransactionCommand> _validator;

        public CreateTransactionHandler(MyDbContext context, IValidator<CreateTransactionCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<IResult> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var result = _validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }

            var newTransaction = new Transaction(request.sku, request.amount, request.currency);

            _context.Transactions.Add(newTransaction);

            await _context.SaveChangesAsync();

            return Results.Created($"api/transactions/{newTransaction.sku}", null);
        }
    }

    public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionValidator()
        {
            RuleFor(r => r.sku).NotEmpty();
            RuleFor(r => r.amount).NotEmpty();
            RuleFor(r => r.currency).NotEmpty();
        }
    }
}
