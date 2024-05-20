using AutoMapper;
using DesafioBackEnd.Application.Deliveryman.Register;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Application.Order.RegisterOrder;
using DesafioBackEnd.Domain.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace DesafioBackEnd.Application.Order.ListDeliverymen;

public class ListDeliverymanCommandHandler : IRequestHandler<ListDeliverymanCommand, ListDeliverymanOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public ListDeliverymanCommandHandler(
        IMapper mapper,
        IOrderRepository orderRepository,
        IConfiguration configuration)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _configuration = configuration;
    }

    public async Task<ListDeliverymanOrderResponse> Handle(ListDeliverymanCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindOrderByIdWithRelationshipAsync(request.orderId);

        if (order is null)
        {
            throw new NotFoundException("Order was not found.");
        }

        var baseUrl = _configuration["AzureBlobStorage:BaseUri"];

        return new ListDeliverymanOrderResponse()
        {
            Order = _mapper.Map<OrderResponse>(order),
            Deliverymen = order
                .Notifications
                .Select(n => new DeliverymanResponse(
                    n.UserId,
                    n.User.Name,
                    n.User.Email,
                    n.User.DeliveryDetail.Cnpj,
                    n.User.DeliveryDetail.Birthdate,
                    n.User.DeliveryDetail.Cnh,
                    n.User.DeliveryDetail.CnhType,
                    $"{baseUrl}/{n.User.DeliveryDetail.CnhImageName}"
                ))
                .ToList(),
        };
    }
}
