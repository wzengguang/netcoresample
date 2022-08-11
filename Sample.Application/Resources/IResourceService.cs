using Microsoft.AspNetCore.Http;

namespace Sample.Application.Resources
{
    public interface IResourceService
    {
        Task<ServiceResult> Upload(IFormFile formFile);
    }
}
