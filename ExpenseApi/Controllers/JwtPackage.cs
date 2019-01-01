using System;

namespace ExpenseApi.Controllers
{
    public class JwtPackage
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}