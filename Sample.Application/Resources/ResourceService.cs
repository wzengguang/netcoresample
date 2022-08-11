using Sample.Application.Resources.Models;
using Sample.Application.Session;
using Sample.Application.Upload;
using Sample.Common.Utils;
using Sample.Domain.Accounts;
using Sample.Domain.Resources;
using Microsoft.AspNetCore.Http;

namespace Sample.Application.Resources
{
    public class ResourceService : ServiceBase, IResourceService
    {
        private readonly IResourceReposity _resourceReposity;

        private readonly IUploadService _uploadService;
        public ResourceService(
            IUploadService uploadService,
            IResourceReposity resourceService,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _uploadService = uploadService;
            _resourceReposity = resourceService;
        }

        public async Task<ServiceResult> Upload(IFormFile formFile)
        {
            if (formFile.Length == 0)
            {
                return new ServiceResult(ServiceResultCodeEnum.error, null);
            }

            MediaModel fileModel = new MediaModel(formFile);
            await fileModel.CreateMD5();

            if (!fileModel.IsLegal)
            {
                return new ServiceResult(ServiceResultCodeEnum.custom, ServiceResultConst.unlegalMediaType);
            }

            Resource resource = await _uploadService.UploadMedia(fileModel);

            return new ServiceResult(_uploadService.GetMediaUrl(resource.ServerFilePath));
        }
    }
}
