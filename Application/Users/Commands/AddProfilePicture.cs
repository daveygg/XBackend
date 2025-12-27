using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands;
public class AddProfilePicture : IRequest
{
    public required IFormFile ProfilePicture { get; set; }
    public required string Token { get; set; }
}
