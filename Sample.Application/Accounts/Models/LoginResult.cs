namespace Sample.Application.Accounts.Models
{
    public class LoginResult
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
