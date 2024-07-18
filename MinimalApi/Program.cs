using Application.Posts.Commands;
using Application.Posts.Queries;
using DataAccess;
using Domain.Models;
using MediatR;
using MinimalApi.EndpointDefinitions;
using MinimalApi.Extensions;
using MinimalApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseOptions();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}
app.UseAntiforgery();

app.RegisterEndpointDefinitions();

app.Run();