using MediatR;

namespace DesafioBackEnd.Application.Order.ListDeliverymen;

public record ListDeliverymanCommand(Guid orderId) : IRequest<ListDeliverymanOrderResponse>;
