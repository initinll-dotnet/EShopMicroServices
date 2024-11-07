using BuildingBlocks.CQRS;

using FluentValidation;

using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(OrderDto OrderDto) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderDto.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.OrderDto.CustomerId).NotNull().WithMessage("CustomerId is required");
        RuleFor(x => x.OrderDto.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");
    }
}
