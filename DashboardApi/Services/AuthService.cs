using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DashboardApi.Models;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace DashboardApi.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Authenticate(UserDto userDto);
        Task Register(UserDto userDto);
        Task<string> RefreshAccessToken(RefreshTokenDto refreshTokenDto);
    }

    public class AuthService : IAuthService
    {
        private readonly string _jwtSecret;
        private readonly IDbConnection _dbConnection;
        private ILogger<AuthService> _logger;

        public AuthService(IDbConnection dbConnection, ILogger<AuthService> logger, string jwtSecret)
        {
            _jwtSecret = jwtSecret;
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public async Task<AuthResponse> Authenticate(UserDto userDto)
        {
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>("SELECT id, email, password_hash FROM users WHERE email=@email",
                new { email = userDto.Email});
            if (user == null || !BC.Verify(userDto.Password, user.PasswordHash))
                throw new ServiceException(401, "Wrong Username or Password");

            var accessToken = GenerateAccessToken(claims: new[] {new Claim(ClaimTypes.Name, userDto.Email)});
            var refreshToken = await GenerateRefreshToken(user.Id);

            return new AuthResponse {AccessToken = accessToken, RefreshToken = refreshToken};
        }

        public async Task<string> RefreshAccessToken(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = await _dbConnection.QueryFirstOrDefaultAsync<RefreshToken>(
                "SELECT token, valid_until, expired, u.email as user_email FROM refresh_tokens JOIN users u on refresh_tokens.user_id = u.id WHERE token=@token",
                new {token = refreshTokenDto.RefreshToken});

            if (refreshToken == null || refreshToken.ValidUntil <= DateTime.UtcNow || refreshToken.Expired)
            {
                throw new ServiceException(401, "Invalid refresh token");
            }

            var accessToken = GenerateAccessToken(new[] {new Claim(ClaimTypes.Name, refreshToken.UserEmail)});

            return accessToken;
        }

        async Task<string> GenerateRefreshToken(int userId)
        {
            string refreshToken;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);

                refreshToken = Convert.ToBase64String(tokenData);
            }

            await _dbConnection.ExecuteAsync(
                "INSERT INTO refresh_tokens(user_id, valid_until, token) VALUES (@userId, @validUntil, @token)",
                new {userId = userId, validUntil = DateTime.UtcNow.AddDays(7), token = refreshToken});
            return refreshToken;
        }

        string GenerateAccessToken(IEnumerable<Claim> claims)
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

        public async Task Register(UserDto userDto)
        {
            if (await _dbConnection.QueryFirstOrDefaultAsync("SELECT email FROM users WHERE email=@email",
                new {email = userDto.Email}) != null)
            {
                throw new ServiceException(409, "User already exists");
            }

            var hashedPassword = BC.HashPassword(userDto.Password, BC.GenerateSalt());
            await _dbConnection.ExecuteAsync("INSERT INTO users(email, password_hash) VALUES (@email, @password_hash)",
                new {email = userDto.Email, password_hash = hashedPassword});
        }
    }

    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}