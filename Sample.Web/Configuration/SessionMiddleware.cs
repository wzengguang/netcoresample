using Sample.Application.Session;
using Sample.Domain.Accounts;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Sample.Web.Configuration
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ISessionService sessionService, IUserRepository userRepository)
        {
#if DEBUG
            var dbid = userRepository.DbContext.ContextId;
#endif

            var token = await context.GetTokenAsync("Bearer", "access_token");

            if (token != null)
            {
                try
                {
                    var tokenObj = new JwtSecurityToken(token);
                    var claimsIdentity = new ClaimsIdentity(tokenObj.Claims);

                    string useName = claimsIdentity.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Name).Value;

                    User user = await userRepository.SingleOrDefaultAsync(a => a.UserName == useName);

                    sessionService.AppSession = new AppSession() { User = user, Token = token };
                }
                catch (Exception)
                {

                }
            }
            await _next(context);
        }
    }
}
