using MediatR;

namespace DesafioBackEnd.Application.Order.ListDeliverymen;

public record ListDeliverymanQuery(Guid orderId) : IRequest<ListDeliverymanOrderResponse>;
