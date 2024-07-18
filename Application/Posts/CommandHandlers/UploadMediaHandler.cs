using Application.Posts.Commands;
using MediatR;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.CommandHandlers;
public class UploadMediaHandler : IRequestHandler<UploadMedia, string>
{
    public Task<string> Handle(UploadMedia request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
