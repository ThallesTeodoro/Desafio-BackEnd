using MediatR;

namespace DesafioBackEnd.Application.Order.RegisterOrder;

public record RegisterOrderCommand(decimal Value) : IRequest<OrderResponse>;
