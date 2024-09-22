using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Application.Abstractions;

namespace Infrastructure.BlobStorage;
public sealed class BlobStorageHelper(BlobServiceClient blobServiceClient) : IBlobStorageHelper
{
    private const string ContainerName = "images";

    public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        var response = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);

        return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
    }

    public async Task<Guid> UploadAsync(IFormFile media, string contentType, CancellationToken cancellationToken = default)
    {
        Stream stream = media.OpenReadStream();
        Guid fileId = Guid.NewGuid();

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());


        await blobClient.UploadAsync(
        stream,
        new BlobHttpHeaders { ContentType = contentType },
        cancellationToken: cancellationToken);


        return fileId;
    }
}
