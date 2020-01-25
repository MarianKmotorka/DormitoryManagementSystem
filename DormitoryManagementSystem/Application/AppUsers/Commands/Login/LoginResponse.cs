namespace Application.AppUsers.Commands.Login
{
    public class LoginResponse
    {
        public string Jwt { get; set; }
        public string RefreshToken { get; set; }
    }
}
