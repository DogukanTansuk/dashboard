using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DashboardApi.Models
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}