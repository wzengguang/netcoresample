using Sample.Domain.Accounts;

namespace Sample.Application.Session
{
    public class AppSession
    {
        public User User { get; set; }

        public long UserId { get { return User.Id; } }

        public string WeAppOpenId { get { return User.WeAppOpenId; } }

        public string Token { get; set; }
    }
}
