namespace DesafioBackEnd.Application.Deliveryman.Register;

public record DeliverymanResponse(
    Guid Id,
    string Name,
    string Email,
    string Cnpj,
    DateOnly Birthdate,
    string Cnh,
    short CnhType,
    string CnhImageName
);