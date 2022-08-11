using Sample.Application;
using Sample.Application.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService resourceService;

        public ResourceController(IResourceService resourceService)
        {
            this.resourceService = resourceService;
        }

        [HttpPost]
        public async Task<ServiceResult> Upload(IFormFile file)
        {
            return await resourceService.Upload(file);
        }
    }
}
