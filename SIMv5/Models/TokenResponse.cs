namespace SIM.Models
{
    using System;
    public class TokenResponse
    {
      
            public string Token { get; set; }

            public User User { get; set; }

            public DateTime Expiration { get; set; }

            public DateTime ExpirationLocal => Expiration.ToLocalTime();

    }
}
