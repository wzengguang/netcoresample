using Sample.Application.Session;
using Sample.Common.Extensions;
using Sample.Common.Utils;
using Sample.Domain.Accounts;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Sample.Web.Configuration
{
    public class ResourcePathRewriteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ResourcePathRewriteMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var path = context.Request.Path.Value;
                var token = context.Request.Query["token"].ToString();

                if (path.Contains(_configuration.ResourceRoot()))
                {
                    var fileName = Path.GetFileNameWithoutExtension(path);

                    var encodeFileName = string.IsNullOrEmpty(token) ? SecurityHelper.AesDecrypt(fileName, _configuration.JWTSecret()) : SecurityHelper.AesDecrypt(fileName, token.Take(8).ToString());

                    context.Request.Path = path.Replace(fileName, encodeFileName);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            await _next(context);
        }
    }
}
