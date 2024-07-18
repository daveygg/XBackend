using Application.Abstractions;
using Application.Posts.Queries;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.QueryHandlers;
public class GetAllPosts2Handler : IRequestHandler<GetAllPosts2, ICollection<Post2>>
{
    private readonly IPostRepository _postsRepo;
    public GetAllPosts2Handler(IPostRepository postsRepo)
    {
        _postsRepo = postsRepo;
    }
    public async Task<ICollection<Post2>> Handle(GetAllPosts2 request, CancellationToken cancellationToken)
    {
        return await _postsRepo.GetAllPosts2();
    }
}
