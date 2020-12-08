using System.ComponentModel.DataAnnotations;

namespace DashboardApi.Models
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}