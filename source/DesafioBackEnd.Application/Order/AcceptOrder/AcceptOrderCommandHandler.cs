using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Enums;
using MediatR;

namespace DesafioBackEnd.Application.Order.AcceptOrder;

public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptOrderCommandHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindOrderByIdWithRelationshipAsync(request.orderId);

        if (order is null)
        {
            throw new NotFoundException("Order was not found.");
        }

        var userIsAbleToAccept = order.Notifications.Any(n => n.UserId == request.userId);

        if (!userIsAbleToAccept || order.Status != OrderStatusEnum.Available)
        {
            throw new ForbiddenException("User is not able to accept the order.");
        }

        order.UserId = request.userId;
        order.Status = OrderStatusEnum.Accepted;
        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
