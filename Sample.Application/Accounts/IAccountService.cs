using Sample.Application.Accounts.Models;
using Microsoft.AspNetCore.Http;

namespace Sample.Application.Accounts
{
    public interface IAccountService
    {
        Task<UserInfo> GetUserInfo();
        Task<string> UpdateAvatar(IFormFile formFile);
        Task<UserInfo> UpdateGender(UserInfo userInfo);
        Task<UserInfo> UpdateNickName(UserInfo userInfo);
    }
}