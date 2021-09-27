namespace UsersManagementMicroservice.Models.Account
{
    public class AuthenticateResult
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
