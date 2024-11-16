using Application.Posts.Commands;
using Application.Posts.Queries;
using Domain.Models;
using MediatR;
using MinimalApi.EndpointDefinitions;
using MinimalApi.Extensions;
using MinimalApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseOptions();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseAuthentication();

app.UseAuthorization();

app.RegisterEndpointDefinitions();

//app.MapIdentityApi<User>();

app.Run();