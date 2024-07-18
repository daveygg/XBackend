using Application.Abstractions;
using Application.Posts.Commands;
using Domain.Models;
using MediatR;
using Infrastructure;

namespace Application.Posts.CommandHandlers;
public class CreatePostHandler2 : IRequestHandler<CreatePost2, Post2>
{
    private readonly IPostRepository _postsRepo;
    private readonly IBlobStorageHelper _blobStorageHelper;
    public CreatePostHandler2(IPostRepository postsRepo, IBlobStorageHelper blobStorageHelper)
    {
        _postsRepo = postsRepo;
        _blobStorageHelper = blobStorageHelper;
    }
    public async Task<Post2> Handle(CreatePost2 request, CancellationToken cancellationToken)
    {        
        var newPost = new Post2
        {
            Content = request.PostContent,
            DateCreated = DateTime.Now,
            LastModified = DateTime.Now
        };

        var media = request.Media;

        if (media != null)
        {
            Guid path = await _blobStorageHelper.UploadAsync(media, media.ContentType);

            newPost.MediaPath = path;
        }

        return await _postsRepo.CreatePost2(newPost);
    }
}
