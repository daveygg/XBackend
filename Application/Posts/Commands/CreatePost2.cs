using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Posts.Commands;
public class CreatePost2 : IRequest<Post>
{
    public string? PostContent { get; set; }
    public string? MediaPath { get; set; }
}
