using Application.Abstractions;
using Application.Posts.Commands;
using Application.Posts.Queries;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Abstractions;
using MinimalApi.Filters;

namespace MinimalApi.EndpointDefinitions;

public class UserEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var users = app.MapGroup("/api/users");
        
        users.MapGet("/{id}", GetUserById)
            .WithName("GetUserById");
        users.MapPost("/register", RegisterUser)
            .WithName("RegisterUser");
        users.MapPost("/login", Login)
            .WithName("LoginUser");
    }
    
    private async Task<IResult> GetUserById(IMediator mediator,
        string id)
    {
        var getUser = new GetUserById { UserId = id };
        var user = await mediator.Send(getUser);
        
        if (user is not null)
        {
            return TypedResults.Ok(user);
        }
        else
        {
            return TypedResults.NotFound();
        }
    }

    private async Task<IResult> Login(IMediator mediator,
        string username,
        string password)
    {
        var signInUser = new SignInUser
        {
            Username = username,
            Password = password
        };
        var jwtResult = await mediator.Send(signInUser);

        return TypedResults.Ok(jwtResult);
    }
    private async Task<IResult> RegisterUser(IMediator mediator,
        string email,
        string password,
        string username)
    {
        var registerUser = new RegisterUser
        {
            Email = email,
            Password = password,
            Username = username
        };
        await mediator.Send(registerUser);
        return TypedResults.Ok();
    }
}
