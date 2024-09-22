using Application.Abstractions;
using Application.Posts.Queries;
using Application.Users.Queries;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.QueryHandlers;
public class GetUserByIdHandler : IRequestHandler<GetUserById, User>
{
    private IUserRepository _userRepo;

    public GetUserByIdHandler(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }
    public async Task<User> Handle(GetUserById request, CancellationToken cancellationToken)
    {
        return await _userRepo.GetUserById(request.UserId);
    }
}
