using Sample.Application.Accounts.Models;
using Sample.Domain.Accounts;

namespace Sample.Application.Accounts
{
    public interface IAuthService
    {
        Task<User> GetUserInfoByWeiXin(string weapp);
        Task<LoginResult> Login(LoginModel model);
        Task<RegisterResult> Register(RegisterModel model);
        Task<LoginResult> WxLogin(string code);
    }
}
