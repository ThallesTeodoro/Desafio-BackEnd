using AutoMapper;
using DesafioBackEnd.Domain.Contracts.EventBus;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Contracts.QueueEvents;
using DesafioBackEnd.Domain.Enums;
using MediatR;

namespace DesafioBackEnd.Application.Order.RegisterOrder;

public class RegisterOrderCommandHandler : IRequestHandler<RegisterOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;

    public RegisterOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IEventBus eventBus,
        IMapper mapper,
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _eventBus = eventBus;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<OrderResponse> Handle(RegisterOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Domain.Entities.Order()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatusEnum.Available,
            Value = request.Value,
        };

        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var availableDeliveryman = await _userRepository.GetAvailableDeliveryman();

        foreach (var deliveryman in availableDeliveryman)
        {
            await _eventBus.PublishAsync(new NotificationOrderEvent(order.Id, deliveryman.Id), cancellationToken);
        }

        return _mapper.Map<OrderResponse>(order);
    }
}
