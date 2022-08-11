namespace Sample.Application.Accounts.Models
{
    public class RegisterResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public RegisterResult(bool success, string message)
        {
            this.Message = message;
            this.IsSuccess = success;
        }
    }
}
