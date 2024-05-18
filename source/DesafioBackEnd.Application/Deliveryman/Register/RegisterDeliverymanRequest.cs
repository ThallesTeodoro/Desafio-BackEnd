using Microsoft.AspNetCore.Http;

namespace DesafioBackEnd.Application.Deliveryman.Register;

public record RegisterDeliverymanRequest(string Name, string Email, string Cnpj, DateOnly Birthdate, string Cnh, string CnhType, IFormFile CnhImage);
