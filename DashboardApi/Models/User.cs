using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DashboardApi
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}