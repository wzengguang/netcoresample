namespace Sample.Domain.Accounts
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
