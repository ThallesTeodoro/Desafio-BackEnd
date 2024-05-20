namespace DesafioBackEnd.Application.Bikes.List;

public record ListBikeRequest(int? Page, int? PageSize, string? Plate);
