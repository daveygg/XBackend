using Application.Abstractions;
using Application.Posts.Commands;
using Domain.Models;
using Media;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.CommandHandlers;
public class CreatePostHandler2 : IRequestHandler<CreatePost, Post>
{
    private readonly IPostRepository _postsRepo;
    public async Task<Post2> Handle(CreatePost2 request, CancellationToken cancellationToken)
    {
        var newPost = new Post2
        {
            Content = request.PostContent,
            DateCreated = DateTime.Now,
            LastModified = DateTime.Now,
            MediaPath = request.MediaPath
        }; 

        return await _postsRepo.CreatePost2(newPost);
    }

    public Task<Post> Handle(CreatePost request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
