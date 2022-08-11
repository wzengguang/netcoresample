using Microsoft.AspNetCore.Mvc;
using Sample.Application.Accounts;
using Sample.Domain.Accounts;
using Sample.Application.Accounts.Models;

namespace Sample.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private IAuthService authService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            this.authService = authService;
        }

        [HttpGet]
        public async Task<User> GetUserByWeApp(string openId)
        {
            User user = await authService.GetUserInfoByWeiXin(openId);

            return user;
        }

        [HttpGet]
        public async Task<LoginResult> WeappLogin(string code)
        {
            return await authService.WxLogin(code);
        }

        [HttpPost]
        public async Task<RegisterResult> Register([FromBody] RegisterModel model)
        {
            return await authService.Register(model);
        }

        [HttpPost]
        public async Task<LoginResult> Login([FromBody] LoginModel model)
        {
            return await authService.Login(model);
        }
    }
}