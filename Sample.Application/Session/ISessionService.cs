using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Sample.Application.Session
{
    public interface ISessionService
    {
        AppSession AppSession { get; set; }
        IConfiguration Configuration { get; }
    }
}
