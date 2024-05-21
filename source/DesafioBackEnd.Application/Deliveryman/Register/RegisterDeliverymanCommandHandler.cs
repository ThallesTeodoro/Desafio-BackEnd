using System.Text.RegularExpressions;
using AutoMapper;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Contracts.Services;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace DesafioBackEnd.Application.Deliveryman.Register;

public class RegisterDeliverymanCommandHandler : IRequestHandler<RegisterDeliverymanCommand, DeliverymanResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IDeliverymanRepository _deliverymanRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IStorageService _storageService;

    public RegisterDeliverymanCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IStorageService storageService,
        IDeliverymanRepository deliverymanRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _storageService = storageService;
        _deliverymanRepository = deliverymanRepository;
    }

    public async Task<DeliverymanResponse> Handle(RegisterDeliverymanCommand request, CancellationToken cancellationToken)
    {
        using Stream stream = request.CnhImage.OpenReadStream();
        var fileName = await _storageService.UploadFileAsync(stream, "cnh", Path.GetExtension(request.CnhImage.FileName), request.CnhImage.ContentType);

        var roles = await _roleRepository.AllAsync();
        var user = new Domain.Entities.User()
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = request.Name,
            RoleId = roles
                .First(r => r.Name == RoleEnum.Deliveryman)
                .Id,
        };

        var deliverymanDetails = new DeliveryDetail()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Cnh = request.Cnh,
            CnhType = CnhTypeEnum.GetCnhType(request.CnhType),
            Cnpj = Regex.Replace(request.Cnpj, @"[^\d]", string.Empty),
            CnhImageName = fileName,
            Birthdate = request.Birthdate,
        };

        await _userRepository.AddAsync(user);
        await _deliverymanRepository.AddAsync(deliverymanDetails);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var baseUrl = _configuration["AzureBlobStorage:BaseUri"];
        var imageUrl = $"{baseUrl}/{deliverymanDetails.CnhImageName}";

        return new DeliverymanResponse(
            user.Id,
            user.Name,
            user.Email,
            deliverymanDetails.Cnpj,
            deliverymanDetails.Birthdate,
            deliverymanDetails.Cnh,
            deliverymanDetails.CnhType,
            imageUrl
        );
    }
}
