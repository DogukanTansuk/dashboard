using System.ComponentModel.DataAnnotations;

namespace DashboardApi.Models
{
    public class UserDto
    {
        [Required]
        public string Email { get; }
        [Required]
        public string Password { get; }
    }
}