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
        users.MapPost("/", RegisterUser)
            .WithName("RegisterUser");
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

    private async Task<IResult> RegisterUser(IMediator mediator,
        string email,
        string password)
    {
        var registerUser = new RegisterUser { Email = email, Password = password };
        await mediator.Send(registerUser);
        return TypedResults.Ok();
    }
}
