using Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.BlobStorage;
public class LocalStorageHelper : IBlobStorageHelper
{
    private readonly string _uploadDirectory;

    public LocalStorageHelper(IConfiguration configuration)
    {
        _uploadDirectory = configuration["UploadDirectory"]!;
    }

    public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_uploadDirectory, fileId.ToString());
        if (File.Exists(filePath))
        {
            await Task.Run(() => File.Delete(filePath), cancellationToken);
        }
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        string filePath = Path.Combine(_uploadDirectory, fileId.ToString());
        if (!File.Exists(filePath))
        {
            throw new Exception("File Missing?!");
        }

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);

        var contentType = GetContentType(filePath);

        return new FileResponse(fileStream, contentType);
    }


    public async Task<Guid> UploadAsync(IFormFile media, string contentType, CancellationToken cancellationToken = default)
    {
        if (media == null || media.Length == 0)
        {
            throw new ArgumentNullException(nameof(media), "File is null or empty.");
        }

        var fileName = Guid.NewGuid().ToString();
        var filePath = Path.Combine(_uploadDirectory, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await media.CopyToAsync(stream, cancellationToken);
        }

        return new Guid(fileName);
    }

    private string GetContentType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();

        return extension switch
        {
            ".txt" => "text/plain",
            ".jpg" => "image/jpeg",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            ".mp4" => "video/mp4",
            _ => "application/octet-stream"
        };
    }
}
