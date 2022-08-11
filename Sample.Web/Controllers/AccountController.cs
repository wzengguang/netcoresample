using Sample.Application.Accounts;
using Sample.Application.Accounts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<UserInfo> GetUserInfo()
        {
            return await _accountService.GetUserInfo();
        }

        [HttpPost]
        public async Task<UserInfo> UpdateNickName([FromBody] UserInfo userInfo)
        {
            return await _accountService.UpdateNickName(userInfo);
        }

        [HttpPost]
        public async Task<UserInfo> UpdateGender([FromBody] UserInfo userInfo)
        {
            return await _accountService.UpdateGender(userInfo);
        }

        [HttpPost]
        public async Task<string> UpdateAvatar(IFormFile file)
        {
            return await _accountService.UpdateAvatar(file);
        }
    }
}
