using System.ComponentModel.DataAnnotations;

namespace DashboardApi.Models
{
    public class UserDTO
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}