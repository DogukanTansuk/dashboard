using System;

namespace DashboardApi.Models
{
    public class RefreshToken
    {
        public string Token { get; }
        public DateTime ValidUntil { get; }
        public bool Expired { get; }
        public string UserEmail { get; }
    }
}