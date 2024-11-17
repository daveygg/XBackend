using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands;
public class SignInUser : IRequest<string>
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
