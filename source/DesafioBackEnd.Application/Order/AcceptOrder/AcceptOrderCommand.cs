using MediatR;

namespace DesafioBackEnd.Application.Order.AcceptOrder;

public record AcceptOrderCommand(Guid orderId, Guid userId) : IRequest;
