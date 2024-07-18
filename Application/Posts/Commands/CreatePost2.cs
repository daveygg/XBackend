using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Posts.Commands;
public class CreatePost2 : IRequest<Post2>
{
    public string? PostContent { get; set; }
    //public string? MediaPath { get; set; }

    public IFormFile Media { get; set; }
}
