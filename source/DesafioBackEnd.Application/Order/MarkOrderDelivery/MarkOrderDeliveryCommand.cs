using MediatR;

namespace DesafioBackEnd.Application.Order.MarkOrderDelivery;

public record MarkOrderDeliveryCommand(Guid orderId, Guid userId) : IRequest;
