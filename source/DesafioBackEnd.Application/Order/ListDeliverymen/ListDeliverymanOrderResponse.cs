using DesafioBackEnd.Application.Deliveryman.Register;
using DesafioBackEnd.Application.Order.RegisterOrder;

namespace DesafioBackEnd.Application.Order.ListDeliverymen;

public class ListDeliverymanOrderResponse
{
    public OrderResponse Order { get; set; }
    public List<DeliverymanResponse> Deliverymen { get; set; }
}