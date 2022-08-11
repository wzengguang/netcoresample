using Sample.Application.Accounts.Models;
using Sample.Application.Session;
using Sample.Common.Extensions;
using Sample.Domain.Accounts;
using Sample.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace Sample.Application.Accounts
{
    public class AuthService : ServiceBase, IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository,
            UserManager<User> userManager,
            RoleManager<Role> roleManager, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<RegisterResult> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return new RegisterResult(false, "exist current useName");
            }

            User user = new()
            {
                Id = NewId(),
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Directory = Path.GetRandomFileName(),
                NickName = model.Username,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            return new RegisterResult(result.Succeeded, string.Join(";", result.Errors.Select(e => e.Description)));
        }

        public async Task<LoginResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await GetTokenAsync(user);

                return new LoginResult()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    IsSuccess = true
                };
            }
            return new LoginResult() { IsSuccess = false, Message = "user or password error" };
        }

        public async Task<User> GetUserInfoByWeiXin(string weappOpenId)
        {
            return await _userRepository.SingleOrDefaultAsync(a => a.WeAppOpenId == weappOpenId);
        }

        public async Task<LoginResult> WxLogin(string code)
        {
            HttpClient client = new HttpClient();

            var weiXinLoginResult = await client.GetFromJsonAsync<WeappAuthorizationResult>(GetWxAuthorizationUrl(code));

            var findUser = await _userRepository.SingleOrDefaultAsync(a => a.UserName == weiXinLoginResult.openid);
            if (findUser == null)
            {
                User user = new()
                {
                    Id = NewId(),
                    Email = weiXinLoginResult.openid + "@None.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = weiXinLoginResult.openid,
                    WeAppOpenId = weiXinLoginResult.openid,
                    WeAppSessionKey = weiXinLoginResult.session_key,
                    Directory = Path.GetRandomFileName(),
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now,
                };

                var result = await _userManager.CreateAsync(user, weiXinLoginResult.openid);
                if (!result.Succeeded)
                {
                    return new LoginResult() { IsSuccess = false, Message = "Create user account false" };
                }
                findUser = await _userRepository.SingleOrDefaultAsync(a => a.UserName == weiXinLoginResult.openid);
            }
            else
            {
                if (findUser == null)
                {
                    findUser.Directory = Path.GetRandomFileName();
                }

                findUser.WeAppOpenId = weiXinLoginResult.openid;
                findUser.WeAppSessionKey = weiXinLoginResult.session_key;
                await _userRepository.SaveChangeAsync();
            }

            getUserDirectory();

            var token = await GetTokenAsync(findUser);
            return new LoginResult()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                IsSuccess = true,
            };
        }

        private async Task<JwtSecurityToken> GetTokenAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.JWTSecret()));

            var token = new JwtSecurityToken(
                issuer: Configuration.JWTIssuer(),
                audience: Configuration.JWTAudience(),
                expires: DateTime.Now.AddHours(Configuration.JWTExpires()),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string GetWxAuthorizationUrl(string code)
        {
            string appid = Configuration.WeappAppId();
            string secreat = Configuration.WeappAppSecret();
            return $"https://api.weixin.qq.com/sns/jscode2session?appid={appid}&secret={secreat}&js_code={code}&grant_type=authorization_code";
        }
    }
}
