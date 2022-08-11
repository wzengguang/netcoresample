namespace Sample.Domain.Resources
{
    public class ResourceReposity : EFRepository<Resource>, IResourceReposity
    {
        public ResourceReposity(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
