using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DashboardApi.Models;
using DashboardApi.Services;
using System.Linq;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;

namespace DashboardApi.Tests.Services
{
    public class TestAuthService: IAuthService
    {
        private readonly string _jwtSecret;
        private List<User> _users;
        private List<RefreshToken> _refreshTokens;

        public TestAuthService(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
            _users = new List<User>();
            _refreshTokens = new List<RefreshToken>();
        }
        public Task<AuthResponse> Authenticate(UserDto userDto)
        {
            var user = _users.SingleOrDefault(u => u.Email == userDto.Email);
            if (user == null || !BC.Verify(userDto.Password, user.PasswordHash))
                throw new ServiceException(401, "Wrong Username or Password");

            var accessToken = AuthUtils.GenerateAccessToken(claims: new[] {new Claim(ClaimTypes.Name, userDto.Email)}, _jwtSecret);
            var refreshToken = AuthUtils.GenerateRefreshToken(user.Id);
            
            _refreshTokens.Append(new RefreshToken
            {
                Token = refreshToken, Expired = false, UserEmail = userDto.Email, ValidUntil = DateTime.Now.AddDays(7)
            });

            return Task.FromResult(new AuthResponse {AccessToken = accessToken, RefreshToken = refreshToken});
        }

        public Task Register(UserDto userDto)
        {
            if (_users.Any(u => u.Email == userDto.Email))
            {
                throw new ServiceException(409, "User already exists");
            }
            var p = BC.HashPassword(userDto.Password, BC.GenerateSalt());
            _users.Append(new User { Id=1, Email = userDto.Email, PasswordHash = p});

            return Task.CompletedTask;
        }

        public Task<string> RefreshAccessToken(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = _refreshTokens.SingleOrDefault(t => t.Token == refreshTokenDto.RefreshToken);

            if (refreshToken == null || refreshToken.ValidUntil <= DateTime.UtcNow || refreshToken.Expired)
            {
                throw new ServiceException(401, "Invalid refresh token");
            }

            var accessToken = AuthUtils.GenerateAccessToken(new[] {new Claim(ClaimTypes.Name, refreshToken.UserEmail)}, _jwtSecret);

            return Task.FromResult(accessToken);
        }
    }
}