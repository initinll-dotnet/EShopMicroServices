using MediatR;

using Microsoft.Extensions.Logging;

using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderUpdatedEventHandler : INotificationHandler<OrderUpdatedEvent>
{
    private readonly ILogger<OrderUpdatedEventHandler> logger;

    public OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
