using AutoMapper;
using Sample.Domain.Accounts;

namespace Sample.Application.Accounts.Models
{
    [AutoMap(typeof(User))]
    public class UserInfo
    {
        public string Id { get; set; }
        public string NickName { get; set; }

        public string AvatarUrl { get; set; }

        public string WeAppOpenId { get; set; }

        public int Gender { get; set; }
    }
}
