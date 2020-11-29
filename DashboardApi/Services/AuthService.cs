using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DashboardApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace DashboardApi.Services
{
    public interface IAuthService
    {
        string Authenticate(UserDTO userDto);
        User Register(UserDTO userDto);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly DashboardContext _context;

        public AuthService(IConfiguration configuration, DashboardContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string Authenticate(UserDTO userDto)
        {
            var user = _context.Users.Single(u => u.email == userDto.email);
            if (user == null) return null;
            if (!BC.Verify(userDto.email, user.passwordHash)) return null;

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"]));
            var token = new JwtSecurityToken(
                claims: new[] {new Claim(ClaimTypes.Name, userDto.email)},
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User Register(UserDTO userDto)
        {
            var user 
        }
    }
}