using Microsoft.AspNetCore.Http;

namespace Media;
public interface IMediaHelper
{
    Task<string> UploadFile(IFormFile media);
}