using Application.Abstractions;
using Application.Posts.Commands;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace DataAccess.Repositories;
public class PostRepository : IPostRepository
{
    private readonly SocialDbContext _ctx;
    //create transient fault handler
    private readonly RetryPolicy _tfh = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategy.DefaultExponential);
    public PostRepository(SocialDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Post> UpdatePost(string updatedContent, int postId)
    {
        return await _tfh.ExecuteAsync<Post>(() => _UpdatePost(updatedContent, postId));
    }
    public async Task DeletePost(int postId)
    {
        await _tfh.ExecuteAsync(() => _DeletePost(postId));
    }
    public async Task<Post> CreatePost(Post toCreate)
    {
        return await _tfh.ExecuteAsync<Post>(() => _CreatePost(toCreate));
    }
    public async Task<Post> GetPostById(int postId)
    {
        return await _tfh.ExecuteAsync(() => _GetPostById(postId));
    }
    public async Task<ICollection<Post>> GetAllPosts()
    {
        return await _tfh.ExecuteAsync<ICollection<Post>>(_GetAllPosts);
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
}