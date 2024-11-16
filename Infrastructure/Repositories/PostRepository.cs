using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions;

namespace Infrastructure.Repositories;
public class PostRepository : IPostRepository
{
    private readonly SocialDbContext _ctx;

    public PostRepository(SocialDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Post?> UpdatePost(string updatedContent, int postId)
    {
        var post = await _ctx.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) { return null; }

        post.LastModified = DateTime.Now;
        post.Content = updatedContent;

        await _ctx.SaveChangesAsync();

        return post;
    }

    public async Task DeleteAllPosts()
    {
        var posts = await _ctx.Posts.ToListAsync();

        foreach (var post in posts)
        {
            _ctx.Remove(post);
        }

        await _ctx.SaveChangesAsync();
    }

    public async Task<Post> CreatePost(Post toCreate)
    {
        toCreate.DateCreated = DateTime.Now;
        toCreate.LastModified = DateTime.Now;

        _ctx.Posts.Add(toCreate);

        await _ctx.SaveChangesAsync();

        return toCreate;
    }

    public async Task<Post?> GetPostById(int postId)
    {
        return await _ctx.Posts.FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<ICollection<Post>> GetAllPosts()
    {
        return await _ctx.Posts.ToListAsync();
    }

    public async Task DeletePost(int postId)
    {
        var post = await _ctx.Posts
            .FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) return;

        _ctx.Posts.Remove(post);

        await _ctx.SaveChangesAsync();
    }
}