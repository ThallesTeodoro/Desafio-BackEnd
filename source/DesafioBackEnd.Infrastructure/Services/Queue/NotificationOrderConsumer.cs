using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Contracts.QueueEvents;
using DesafioBackEnd.Domain.Entities;
using MassTransit;

namespace DesafioBackEnd.Infrastructure.Services.Queue;

public class NotificationOrderConsumer : IConsumer<NotificationOrderEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationOrderConsumer(IUnitOfWork unitOfWork, INotificationRepository notificationRepository)
    {
        _unitOfWork = unitOfWork;
        _notificationRepository = notificationRepository;
    }

    public async Task Consume(ConsumeContext<NotificationOrderEvent> context)
    {
        var message = context.Message;

        await _notificationRepository.AddAsync(new Notification()
        {
            OrderId = message.OrderId,
            UserId = message.DeliverymanId,
        });
        await _unitOfWork.SaveChangesAsync(default);
    }
}
