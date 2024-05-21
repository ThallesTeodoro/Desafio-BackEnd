using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
