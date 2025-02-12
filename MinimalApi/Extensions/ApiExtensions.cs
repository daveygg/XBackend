﻿using Application.Abstractions;
using Application.Posts.Commands;
using Application.Posts.Queries;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Abstractions;
using Microsoft.AspNetCore.Identity;
using Domain.Models;
using Infrastructure.BlobStorage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.JWTService;
using Microsoft.OpenApi.Models;

namespace MinimalApi.Extensions;

public static class ApiExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        //builder.Services.AddSwaggerGen(option =>
        //{
        //    option.SwaggerDoc("v1", new OpenApiInfo { Title = "XBackend", Version = "v1" });
        //    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //    {
        //        In = ParameterLocation.Header,
        //        Description = "Please enter a valid token",
        //        Name = "Authorization",
        //        Type = SecuritySchemeType.Http,
        //        BearerFormat = "JWT",
        //        Scheme = "Bearer"
        //    });
        //    option.AddSecurityRequirement(new OpenApiSecurityRequirement
        //    {
        //        {
        //            new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference
        //                {
        //                    Type=ReferenceType.SecurityScheme,
        //                    Id="Bearer"
        //                }
        //            },
        //            new string[]{}
        //        }
        //    });
        //});



        builder.Services.AddDbContext<SocialDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

        var blobStorageType = builder.Configuration["BlobStorageType"];
        
        switch (blobStorageType)
        {
            case "Local":
                builder.Services.AddScoped<IBlobStorageHelper, LocalStorageHelper>();
                break;
            case "Azure":
                builder.Services.AddScoped<IBlobStorageHelper, BlobStorageHelper>();
                break;
            default:
                builder.Services.AddScoped<IBlobStorageHelper, BlobStorageHelper>();
            break;
        }

        builder.Services.AddScoped<IPostRepository, PostRepository>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();        

        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Services.AddSingleton(_ => new BlobServiceClient(builder.Configuration.GetConnectionString("BlobStorage")));        
        
        builder.Services.AddAuthorization();
        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 1;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
        })
        .AddEntityFrameworkStores<SocialDbContext>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
            options.DefaultScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(options => {            
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!)
                )
            };
        });        

        builder.Services.AddSingleton(_ => new BlobServiceClient(builder.Configuration.GetConnectionString("BlobStorage")));

        builder.Services.AddMediatR(assembly =>
        {
            assembly.RegisterServicesFromAssemblies(typeof(CreatePost).Assembly)
                .RegisterServicesFromAssemblies(typeof(GetAllPosts).Assembly)
                .RegisterServicesFromAssemblies(typeof(GetPostById).Assembly)
                .RegisterServicesFromAssemblies(typeof(DeletePost).Assembly)
                .RegisterServicesFromAssemblies(typeof(UpdatePost).Assembly)
                .RegisterServicesFromAssemblies(typeof(GetAllPosts).Assembly);
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
            .Where(t => t.IsAssignableTo(typeof(IEndpointDefinition))
                && !t.IsAbstract
                && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        foreach (var endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.RegisterEndpoints(app);
        }
    }
}
