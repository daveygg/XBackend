using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Media;
public class MediaHelper : IMediaHelper
{
    private readonly string _uploadDirectory;

    public MediaHelper(IConfiguration configuration)
    {
        _uploadDirectory = configuration["UploadDirectory"];
    }
    public async Task<string> UploadFile(IFormFile media)
    {
        if (media == null || media.Length == 0)
        {
            throw new ArgumentNullException(nameof(media));
        }

        string fileName = Path.GetRandomFileName() + Path.GetExtension(media.FileName);
        string filePath = Path.Combine(_uploadDirectory, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await media.CopyToAsync(stream);
        }

        return filePath;
    }

}
