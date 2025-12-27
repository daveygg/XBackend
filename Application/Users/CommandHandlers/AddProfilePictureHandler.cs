using Application.Users.Commands;
using Domain.Models;
using Application.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Users.CommandHandlers;
public class AddProfilePictureHandler : IRequestHandler<AddProfilePicture>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IBlobStorageHelper _blobStorageHelper;

    public AddProfilePictureHandler(UserManager<User> userManager,
        ITokenService tokenService,
        IBlobStorageHelper blobStorageHelper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _blobStorageHelper = blobStorageHelper;
    }

    public async Task Handle(AddProfilePicture request,
        CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetClaimsPrincipal(request.Token);
        var user = await _userManager.GetUserAsync(principal);
        if (user == null) { return; }
        
        Guid path = await _blobStorageHelper.UploadAsync(request.ProfilePicture, request.ProfilePicture.ContentType);
        user.AvatarUrl = path.ToString();
        
        var result = await _userManager.UpdateAsync(user);

        return;
    }
}
