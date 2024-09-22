using Application.Abstractions;
using Application.Posts.Commands;
using Domain.Models;
using MediatR;

namespace Application.Posts.CommandHandlers;
public class CreatePostHandler : IRequestHandler<CreatePost, Post>
{
    private readonly IPostRepository _postsRepo;
    private readonly IBlobStorageHelper _blobStorageHelper;
    public CreatePostHandler(IPostRepository postsRepo, IBlobStorageHelper blobStorageHelper)
    {
        _postsRepo = postsRepo;
        _blobStorageHelper = blobStorageHelper;
    }
    public async Task<Post> Handle(CreatePost request, CancellationToken cancellationToken)
    {        
        var newPost = new Post
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

        return await _postsRepo.CreatePost(newPost);
    }
}
