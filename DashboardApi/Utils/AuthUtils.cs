using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DashboardApi.Services
{
    public class AuthUtils
    {
        public static string GenerateRefreshToken(int userId)
        {
            string refreshToken;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);

                refreshToken = Convert.ToBase64String(tokenData);
            }
            
            return refreshToken;
        }

        public static string GenerateAccessToken(IEnumerable<Claim> claims, string jwtSecret)
        {
            var now = DateTime.UtcNow;
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret));
            var accessToken = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }
    }
}