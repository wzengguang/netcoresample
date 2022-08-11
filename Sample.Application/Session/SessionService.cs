using AutoMapper;
using Sample.Domain;
using Microsoft.Extensions.Configuration;

namespace Sample.Application.Session
{
    public class SessionService : ISessionService
    {
        public AppSession AppSession { get; set; }

        public IConfiguration Configuration { get; }

        public SessionService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
    }
}
