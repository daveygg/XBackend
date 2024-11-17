using MediatR;

namespace Application.Users.Commands;
public class RegisterUser : IRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Username { get; set; }
}
