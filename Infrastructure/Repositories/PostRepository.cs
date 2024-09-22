using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Application.Abstractions;

namespace Infrastructure.Repositories;
public class PostRepository : IPostRepository
{
    private readonly SocialDbContext _ctx;
    private readonly RetryPolicy _retryHandler = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategy.DefaultExponential);

    public PostRepository(SocialDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Post> UpdatePost(string updatedContent, int postId)
    {
        return await _retryHandler.ExecuteAsync<Post>(() => _UpdatePost(updatedContent, postId));
    }
    public async Task DeletePost(int postId)
    {
        await _retryHandler.ExecuteAsync(() => _DeletePost(postId));
    }
    public async Task<Post> CreatePost(Post toCreate)
    {
        return await _CreatePost(toCreate);
    }
    public async Task<Post> GetPostById(int postId)
    {
        return await _retryHandler.ExecuteAsync(() => _GetPostById(postId));
    }
    public async Task<ICollection<Post>> GetAllPosts()
    {
        return await _retryHandler.ExecuteAsync<ICollection<Post>>(_GetAllPosts);
    }

    public async Task DeleteAllPosts()
    {
        await _retryHandler.ExecuteAsync(_DeleteAllPosts);
    }

    private async Task<Post> _GetPostById(int postId)
    {
        return await _ctx.Posts.FirstOrDefaultAsync(p => p.Id == postId);
    }
    private async Task<Post> _CreatePost(Post toCreate)
    {
        toCreate.DateCreated = DateTime.Now;
        toCreate.LastModified = DateTime.Now;
        _ctx.Posts.Add(toCreate);
        await _ctx.SaveChangesAsync();
        return toCreate;
    }
    private async Task _DeletePost(int postId)
    {
        var post = await _ctx.Posts
            .FirstOrDefaultAsync(p => p.Id == postId);        
        if (post == null) return;        
        _ctx.Posts.Remove(post);
        await _ctx.SaveChangesAsync();
    }
    private async Task<ICollection<Post>> _GetAllPosts()
    {
        return await _ctx.Posts.ToListAsync();
    }
    private async Task<Post> _UpdatePost(string updatedContent, int postId)
    {
        var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        post.LastModified = DateTime.Now;
        post.Content = updatedContent;
        await _ctx.SaveChangesAsync();
        return post;
    }    

    private async Task _DeleteAllPosts()
    {
        var posts = await _ctx.Posts.ToListAsync();
        foreach (var post in posts)
        {
            _ctx.Remove(post);
        }
        await _ctx.SaveChangesAsync();
    }
}