using Carter;

using Mapster;

using MediatR;

using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            // Create a custom config for mapping
            var config = new TypeAdapterConfig();

            config
                .NewConfig<CreateOrderRequest, CreateOrderCommand>()
                .Map(dest => dest.OrderDto, src => src.Order);

            var command = request.Adapt<CreateOrderCommand>(config);

            var result = await sender.Send(command);

            var response = result.Adapt<CreateOrderResponse>();

            return Results.Created($"/orders/{response.Id}", response);
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Order")
        .WithDescription("Create Order");
    }
}

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);
