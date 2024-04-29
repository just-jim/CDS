using FluentValidation;

namespace CDS.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderNumber).NotEmpty();
        RuleFor(x => x.OrderDate).NotEmpty().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
    }
}