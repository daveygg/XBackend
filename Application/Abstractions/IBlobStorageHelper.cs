using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;
public interface IBlobStorageHelper
{
    Task<Guid> UploadAsync(IFormFile media, string contentType, CancellationToken cancellationToken = default);

    Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default);
}
public record FileResponse(Stream Stream, string ContentType);
