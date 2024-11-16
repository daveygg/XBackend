using Application.Users.Commands;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.CommandHandlers;
public class RegisterUserHandler : IRequestHandler<RegisterUser>
{
    private readonly UserManager<User> _userManager;

    public RegisterUserHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        var appUser = new User
        {
            Email = request.Email
        };

        var createUser = await _userManager.CreateAsync(appUser);

        if (createUser.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

            if (roleResult.Succeeded) { return; } else { return; }
        }
    }
}
