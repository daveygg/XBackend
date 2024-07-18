using Application.Abstractions;
using Application.Posts.Commands;
using Application.Posts.Queries;
using DataAccess.Repositories;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Abstractions;
using Infrastructure;
using Azure.Storage.Blobs;

namespace MinimalApi.Extensions;

public static class ApiExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services
            
            .AddEndpointsApiExplorer()
            
            .AddSwaggerGen()
            
            .AddDbContext<SocialDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("Default")))
            
            .AddScoped<IPostRepository, PostRepository>()
            
            .AddScoped<IBlobStorageHelper, BlobStorageHelper>()
            
            .AddAntiforgery()
            
            .AddSingleton(_ => new BlobServiceClient(builder.Configuration.GetConnectionString("BlobStorage")));

        builder.Services.AddMediatR(assembly =>
        {
            assembly.RegisterServicesFromAssemblies(typeof(CreatePost).Assembly)
                .RegisterServicesFromAssemblies(typeof(GetAllPosts).Assembly)
                .RegisterServicesFromAssemblies(typeof(GetPostById).Assembly)
                .RegisterServicesFromAssemblies(typeof(DeletePost).Assembly)
                .RegisterServicesFromAssemblies(typeof(UpdatePost).Assembly)
                .RegisterServicesFromAssemblies(typeof(CreatePost2).Assembly)
                .RegisterServicesFromAssemblies(typeof(GetAllPosts2).Assembly);
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });        
    }

    public static void RegisterEndpointDefinitions(this WebApplication app)
    {
        var endpointDefinitions = typeof(Program).Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IEndpointDefinition)) && !t.IsAbstract && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        foreach (var endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.RegisterEndpoints(app);
        }
    }
}
