using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Contracts.Services;
using MediatR;

namespace DesafioBackEnd.Application.Deliveryman.Update;

public class UpdateDeliverymanCommandHandler : IRequestHandler<UpdateDeliverymanCommand>
{
    private readonly IDeliverymanRepository _deliverymanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;

    public UpdateDeliverymanCommandHandler(IStorageService storageService, IUnitOfWork unitOfWork, IDeliverymanRepository deliverymanRepository)
    {
        _storageService = storageService;
        _unitOfWork = unitOfWork;
        _deliverymanRepository = deliverymanRepository;
    }

    public async Task Handle(UpdateDeliverymanCommand request, CancellationToken cancellationToken)
    {
        var deliverymanDetail = await _deliverymanRepository.FindByUserIdAsync(request.UserId);

        if (deliverymanDetail is null)
        {
            throw new NotFoundException("User was not found");
        }

        var oldImage = deliverymanDetail.CnhImageName;

        using Stream stream = request.CnhImage.OpenReadStream();
        deliverymanDetail.CnhImageName = await _storageService.UploadFileAsync(stream, "cnh", Path.GetExtension(request.CnhImage.FileName), request.CnhImage.ContentType);

        await _storageService.DeleteFileIfExistsAsync(oldImage, cancellationToken);

        _deliverymanRepository.Update(deliverymanDetail);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
