using Microsoft.AspNetCore.Identity;

namespace Sample.Domain.Accounts
{
    public class UserRole : IdentityUserRole<long>, IEntityBase
    {
        public long Id { get; set; }
    }
}
