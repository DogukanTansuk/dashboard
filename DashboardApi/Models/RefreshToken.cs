using System;

namespace DashboardApi.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool Expired { get; set; }
        public string UserEmail { get; set; }
    }
}