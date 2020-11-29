using System.ComponentModel.DataAnnotations;

namespace DashboardApi.Models
{
    public class RefreshTokenDTO
    {
        [Required]
        public string refresh_token { get; set; }
    }
}