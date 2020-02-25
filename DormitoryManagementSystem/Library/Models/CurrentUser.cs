namespace Library.Models
{
    public class CurrentUser
    {
        public string UserName { get; set; }

        public string Role { get; set; }

        public string Jwt { get; set; }

        public string RefreshToken { get; set; }
    }
}
