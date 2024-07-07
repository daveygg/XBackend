using Application.Posts.Commands;
using Application.Posts.Queries;
using DataAccess;
using Domain.Models;
using MediatR;
using MinimalApi.EndpointDefinitions;
using MinimalApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();

app.UseCors("AllowAll");
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
app.UseOptions();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.RegisterEndpointDefinitions();

app.Run();