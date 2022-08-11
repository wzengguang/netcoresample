using Sample.Application.Resources.Models;
using Sample.Domain.Resources;
using Microsoft.AspNetCore.Http;

namespace Sample.Application.Upload
{
    public interface IUploadService
    {
        string GetMediaUrl(Resource resource);
        string GetMediaUrl(string serverFilePath);
        Task<Resource> UploadMedia(MediaModel model);
    }
}