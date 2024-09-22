using Domain.Models;
using MediatR;

namespace Application.Users.Queries;
public class GetUserById : IRequest<User>
{
    public string UserId { get; set; }
}
