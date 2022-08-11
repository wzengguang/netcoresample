using Sample.Application.Accounts.Models;
using Sample.Application.Resources.Models;
using Sample.Application.Upload;
using Sample.Common.Extensions;
using Sample.Common.Utils;
using Sample.Domain.Accounts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application.Accounts
{
    public class AccountService : ServiceBase, IAccountService
    {
        private readonly IUploadService _uploadService;
        private readonly IUserRepository _userRepository;
        public AccountService(IUploadService uploadService, IServiceProvider serviceProvider, IUserRepository userRepository) : base(serviceProvider)
        {
            _uploadService = uploadService;
            _userRepository = userRepository;
        }

        public async Task<UserInfo> GetUserInfo()
        {
            var user = await _userRepository.FindAsync(AppSession.UserId);
            var result = Mapper.Map<UserInfo>(user);
            result.AvatarUrl = _uploadService.GetMediaUrl(user.AvatarFilePath);
            return result;
        }

        public async Task<UserInfo> UpdateNickName(UserInfo userInfo)
        {
            User user = await _userRepository.FindAsync(AppSession.UserId);

            if (string.IsNullOrEmpty(userInfo.NickName.Trim()))
            {
                return null;
            }

            user.NickName = userInfo.NickName;

            await SaveChangeAsync();

            return Mapper.Map<UserInfo>(user);
        }

        public async Task<UserInfo> UpdateGender(UserInfo userInfo)
        {
            User user = await _userRepository.FindAsync(AppSession.UserId);
            user.Gender = userInfo.Gender;

            await SaveChangeAsync();
            return Mapper.Map<UserInfo>(user);
        }

        public async Task<string> UpdateAvatar(IFormFile formFile)
        {
            var resource = await _uploadService.UploadMedia(new MediaModel(formFile));

            var user = await _userRepository.FindAsync(AppSession.UserId);

            user.AvatarFilePath = resource.ServerFilePath;

            await SaveChangeAsync();

            var serverUrl = _uploadService.GetMediaUrl(resource.ServerFilePath);

            return serverUrl;
        }
    }
}
