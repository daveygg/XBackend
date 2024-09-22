using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Posts.Commands;
public class CreatePost : IRequest<Post>
{
    public string? PostContent { get; set; }

    public IFormFile? Media { get; set; }
}
