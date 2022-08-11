using AutoMapper;
using Sample.Application.Session;
using Sample.Common.Extensions;
using Sample.Common.Utils;
using Sample.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Intrinsics.Arm;

namespace Sample.Application
{
    public class ServiceBase : SeviceNewId
    {
        protected IMapper Mapper { get; set; }

        protected IConfiguration Configuration { get; set; }

        protected ISessionService SessionService { get; set; }

        private AppDbContext appDbContext;

        protected IHostingEnvironment hostingEnvironment;

        public AppSession AppSession
        {
            get
            {
                if (SessionService.AppSession == null)
                {
                    throw new NoneAppSessionException("User not login but attrive to get user's data!");
                }
                return SessionService.AppSession;
            }
        }

        public ServiceBase(IServiceProvider serviceProvider)
        {
            SessionService = serviceProvider.GetService<ISessionService>();
            Configuration = serviceProvider.GetService<IConfiguration>();
            Mapper = serviceProvider.GetService<IMapper>();

            appDbContext = serviceProvider.GetService<AppDbContext>();
            hostingEnvironment = serviceProvider.GetService<IHostingEnvironment>();
        }

        protected async Task<int> SaveChangeAsync()
        {
            return await this.appDbContext.SaveChangesAsync();
        }

        protected string getUserDirectory()
        {
            string path = Path.Combine(hostingEnvironment.ContentRootPath, AppSession.User.Directory);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

    }
}
