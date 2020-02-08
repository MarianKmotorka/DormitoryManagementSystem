using System;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; } = Guid.NewGuid().ToString();

        public string JwtId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool Used { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
