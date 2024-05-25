using Application.Posts.Commands;
using Application.Posts.Queries;
using Domain.Models;
using MediatR;
using MinimalApi.EndpointDefinitions;
using MinimalApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    try
    {
        await next();
    }
    catch 
    {
        ctx.Response.StatusCode = 500;
        await ctx.Response.WriteAsync("An error occured");
    }
});

app.UseHttpsRedirection();

app.RegisterEndpointDefinitions();

app.Run();