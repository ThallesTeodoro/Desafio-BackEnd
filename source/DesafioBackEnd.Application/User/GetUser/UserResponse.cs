namespace DesafioBackEnd.Application.User.GetUser;

public class UserResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Profile { get; set; }
    public UserDetailResponse UserDetail { get; set; }
}