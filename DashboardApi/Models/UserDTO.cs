using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DashboardApi.Models
{
    public class UserDto
    {
        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}