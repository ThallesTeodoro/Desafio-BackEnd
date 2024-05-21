using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Enums;
using MediatR;

namespace DesafioBackEnd.Application.Order.MarkOrderDelivery;

public class MarkOrderDeliveryCommandHandler : IRequestHandler<MarkOrderDeliveryCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkOrderDeliveryCommandHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task Handle(MarkOrderDeliveryCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.orderId);

        if (order is null)
        {
            throw new NotFoundException("Order was not found.");
        }

        if (order.UserId != request.userId || order.Status == OrderStatusEnum.Delivered)
        {
            throw new ForbiddenException("User is not able to mark order as delivered.");
        }

        order.Status = OrderStatusEnum.Delivered;
        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
