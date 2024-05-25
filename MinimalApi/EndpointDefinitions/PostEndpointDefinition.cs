using Application.Posts.Commands;
using Application.Posts.Queries;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using MinimalApi.Abstractions;
using MinimalApi.Filters;

namespace MinimalApi.EndpointDefinitions;

public class PostEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var posts = app.MapGroup("/api/posts");

        posts.MapGet("/{id}", GetPostById)
            .WithName("GetPostById");
        posts.MapPost("/", CreatePost)
            .WithName("CreatePost")
            .AddEndpointFilter<PostValidationFilter>();
        posts.MapGet("/", GetAllPosts)
            .WithName("GetAllPosts");
        posts.MapPut("/{id}", UpdatePost)
            .WithName("UpdatePost")
            .AddEndpointFilter<PostValidationFilter>();
        posts.MapDelete("/{id}", DeletePost)
            .WithName("DeletePost");
    }
    private async Task<IResult> GetPostById(IMediator mediator, int id)
    {
        var getPost = new GetPostById { PostId = id };
        var post = await mediator.Send(getPost);
        return Results.Ok(post);
    }

    private async Task<IResult> CreatePost(IMediator mediator, Post post)
    {
        var createPost = new CreatePost { PostContent = post.Content };
        var createdPost = await mediator.Send(createPost);
        return Results.CreatedAtRoute("GetPostById", new { createdPost.Id }, createdPost);
    }

    private async Task<IResult> GetAllPosts(IMediator mediator)
    {
        var getAllPosts = new GetAllPosts();
        var posts = await mediator.Send(getAllPosts);
        return TypedResults.Ok(posts);
    }

    private async Task<IResult> UpdatePost(IMediator mediator, Post post, int id)
    {
        var updatePost = new UpdatePost { PostId = id, PostContent = post.Content };
        var updatedPost = await mediator.Send(updatePost);
        return TypedResults.Ok(updatedPost);
    }

    private async Task<IResult> DeletePost(IMediator mediator, int id)
    {
        var deletePost = new DeletePost { PostId = id };
        await mediator.Send(deletePost);
        return TypedResults.NoContent();
    }
}
