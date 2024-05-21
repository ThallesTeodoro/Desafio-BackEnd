namespace DesafioBackEnd.Domain.Contracts.QueueEvents;

public record NotificationOrderEvent(Guid OrderId, Guid DeliverymanId);
