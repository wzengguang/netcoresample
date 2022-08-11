using Microsoft.Extensions.Configuration;

namespace Sample.Common.Extensions
{
    public static class ConfigrationExtension
    {
        public static string JWTIssuer(this IConfiguration configuration)
        {
            return configuration["JWT:Issuer"];
        }

        public static string JWTAudience(this IConfiguration configuration)
        {
            return configuration["JWT:Audience"];
        }

        public static int JWTExpires(this IConfiguration configuration)
        {
            return Convert.ToInt32(configuration["JWT:Expires"]);
        }

        public static string JWTSecret(this IConfiguration configuration)
        {
            return configuration["JWT:Secret"];
        }

        public static string WeappAppId(this IConfiguration configuration)
        {
            return configuration["Weapp:AppId"];
        }

        public static string WeappAppSecret(this IConfiguration configuration)
        {
            return configuration["Weapp:AppSecret"];
        }

        public static string ResourceRoot(this IConfiguration configuration)
        {
            return configuration["ResourcePath:Root"];
        }
    }
}
