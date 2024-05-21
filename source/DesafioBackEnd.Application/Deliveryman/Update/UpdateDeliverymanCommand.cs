using MediatR;
using Microsoft.AspNetCore.Http;

namespace DesafioBackEnd.Application.Deliveryman.Update;

public record UpdateDeliverymanCommand(Guid UserId, IFormFile CnhImage) : IRequest;
