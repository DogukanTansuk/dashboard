using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DashboardApi.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace DashboardApi.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Authenticate(UserDTO userDto);
        Task<User> Register(UserDTO userDto);
        Task<string> GenerateRefreshToken(Guid userId); 
        string GenerateAccessToken(IEnumerable<Claim> claims);

    }

    public class AuthService : IAuthService
    {
        private readonly string _jwtSecret;
        private readonly DashboardDBContext _dbContext;

        public AuthService(DashboardDBContext dbContext, string jwtSecret)
        {
            _jwtSecret = jwtSecret;
            _dbContext = dbContext;
        }

        public async Task<AuthResponse> Authenticate(UserDTO userDto)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == userDto.email);
            if (user == null) return null;
            if (!BC.Verify(userDto.password, user.PasswordHash)) return null;

            var accessToken = GenerateAccessToken(claims: new []{new Claim(ClaimTypes.Name, userDto.email)});
            var refreshToken = await GenerateRefreshToken(user.Id);

            return new AuthResponse {AccessToken = accessToken, RefreshToken = refreshToken};
        }

        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            string refreshToken;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);

                refreshToken = Convert.ToBase64String(tokenData);
                
            }

            var refreshTokenEntity = new RefreshToken
                {Expired = false, UserId = userId, ValidUntil = DateTime.UtcNow.AddDays(7), Token = refreshToken};
            await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();
            
            return refreshToken;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSecret));
            var accessToken = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        public async Task<User> Register(UserDTO userDto)
        {
            var hashedPassword = BC.HashPassword(userDto.password, BC.GenerateSalt());
            var user = new User {Email = userDto.email, PasswordHash = hashedPassword};
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }

    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}