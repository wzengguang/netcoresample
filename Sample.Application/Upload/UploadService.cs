using Sample.Application.Resources.Models;
using Sample.Application.Session;
using Sample.Common.Extensions;
using Sample.Common.Utils;
using Sample.Domain.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Upload
{
    public class UploadService : SeviceNewId, IUploadService
    {
        private IHostingEnvironment _hostingEnvironment;
        private IConfiguration _configuration;
        private ISessionService _sessionService;
        private IResourceReposity _resourceReposity;

        private AppSession AppSession
        {
            get
            {
                if (_sessionService.AppSession == null)
                {
                    throw new NoneAppSessionException("User not login but attrive to get user's data!");
                }
                return _sessionService.AppSession;
            }
        }

        public UploadService(IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService sessionService, IResourceReposity resourceReposity)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._configuration = configuration;
            this._sessionService = sessionService;
            this._resourceReposity = resourceReposity;
        }

        public string CreateServerFileFullPath(MediaModel media)
        {
            if (media.ServerFileName == null)
            {
                media.ServerFileName = Path.GetRandomFileName();
            }

            media.ServerFilePath = Path.Combine(AppSession.User.Directory, media.ServerFileName + media.extension);

            media.ServerFullFilePath = GetServerFullFilePath(media.ServerFilePath);

            var userDirectory = Path.GetDirectoryName(media.ServerFullFilePath);
            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }

            return media.ServerFullFilePath;
        }

        public string GetServerFullFilePath(string serverFilePath)
        {
            return Path.Combine(_hostingEnvironment.ContentRootPath + _configuration.ResourceRoot().Split('/').Last() + "\\", serverFilePath);
        }

        public string GetMediaUrl(Resource resource)
        {
            return GetMediaUrl(resource.ServerFilePath);
        }

        public string GetMediaUrl(string serverFilePath)
        {
            if (serverFilePath == null)
            {
                return null;
            }

            serverFilePath = SecurityHelper.AesEncrypt(serverFilePath, AppSession.Token);

            string rootUri = _configuration.ResourceRoot();

            return _configuration.ResourceRoot() + "/" + serverFilePath.Replace("\\", "/");
        }

        public async Task<Resource> UploadMedia(MediaModel model)
        {
            Resource fileEntity = await _resourceReposity.FirstOrDefaultAsync(a => a.MD5 == model.MD5 && a.CreatedUserId == AppSession.UserId);

            if (fileEntity == null)
            {
                using (var stream = System.IO.File.Create(CreateServerFileFullPath(model)))
                {
                    await model.FormFile.CopyToAsync(stream);
                }

                fileEntity = new Resource
                {
                    CreatedTime = DateTime.Now,
                    CreatedUserId = AppSession.UserId,
                    MD5 = model.MD5,
                    UploadFileName = model.UploadFileName,
                    FileExtension = model.extension,
                    ServerFileName = model.ServerFileName,
                    ServerFilePath = model.ServerFilePath,
                    Id = NewId(),
                };
                await _resourceReposity.AddAsync(fileEntity);
                await _resourceReposity.SaveChangeAsync();
            }
            else if (fileEntity != null && !File.Exists(GetServerFullFilePath(fileEntity.ServerFilePath)))
            {
                var serverFilePath = GetServerFullFilePath(fileEntity.ServerFilePath);
                using (var stream = System.IO.File.Create(serverFilePath))
                {
                    await model.FormFile.CopyToAsync(stream);
                }
                fileEntity.UpdatedTime = DateTime.Now;
                await _resourceReposity.SaveChangeAsync();
            }
            else
            {
                fileEntity.UpdatedTime = DateTime.Now;
                await _resourceReposity.SaveChangeAsync();
            }
            return fileEntity;
        }
    }
}
