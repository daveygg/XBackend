using Application.Abstractions;
using Application.Posts.Commands;
using Application.Posts.Queries;
using DataAccess.Repositories;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Abstractions;
using Media;

namespace MinimalApi.Extensions;

public static class ApiExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<SocialDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<IMediaHelper, MediaHelper>();
        builder.Services.AddMediatR(assembly =>
        {
            assembly.RegisterServicesFromAssemblies(typeof(CreatePost).Assembly);
            assembly.RegisterServicesFromAssemblies(typeof(GetAllPosts).Assembly);
            assembly.RegisterServicesFromAssemblies(typeof(GetPostById).Assembly);
            assembly.RegisterServicesFromAssemblies(typeof(DeletePost).Assembly);
            assembly.RegisterServicesFromAssemblies(typeof(UpdatePost).Assembly);
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
