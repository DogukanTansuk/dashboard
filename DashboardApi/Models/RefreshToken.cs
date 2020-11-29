using System;
using System.ComponentModel.DataAnnotations;

namespace DashboardApi
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool Expired { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}