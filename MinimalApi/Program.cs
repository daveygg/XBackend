using Application.Abstractions;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<SocialDbContext>(opt => opt.UseSqlServer(cs));
builder.Services.AddScoped<IPostRepository, PostRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();