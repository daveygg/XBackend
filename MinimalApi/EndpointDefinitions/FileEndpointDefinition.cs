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

public class FileEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var files = app.MapGroup("/api/files");
            //.RequireAuthorization();

        files.MapGet("/{id}", GetFileById)
            .WithName("GetFileById");
    }

    private async Task<IResult> GetFileById(IMediator mediator,
        IBlobStorageHelper blobStorageHelper,
        Guid id)
    {
        FileResponse fileResponse = await blobStorageHelper.DownloadAsync(id);
        return Results.File(fileResponse.Stream, fileResponse.ContentType);
    }    
}
