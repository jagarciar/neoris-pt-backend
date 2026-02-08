using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Responses;
using NeorisBackend.Services.Interfaces;

namespace NeorisBackend.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de Autenticación
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secret;
        private readonly int _expirationSeconds;
        private readonly string _expectedUsername;
        private readonly string _expectedPassword;

        public AuthService()
        {
            _issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            _audience = ConfigurationManager.AppSettings["JwtAudience"];
            _secret = ConfigurationManager.AppSettings["JwtSecret"];
            _expirationSeconds = int.Parse(ConfigurationManager.AppSettings["JwtExpirationSeconds"]);
            _expectedUsername = ConfigurationManager.AppSettings["AuthUsername"];
            _expectedPassword = ConfigurationManager.AppSettings["AuthPassword"];
        }

        public LoginResponseDto Login(LoginRequestDto dto)
        {
            if (!ValidateCredentials(dto.Username, dto.Password))
            {
                return null;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var expires = now.AddSeconds(_expirationSeconds);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: new[] { new Claim(ClaimTypes.Name, dto.Username) },
                notBefore: now,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseDto
            {
                AccessToken = tokenString,
                TokenType = "Bearer",
                ExpiresAtUtc = expires
            };
        }

        public bool ValidateCredentials(string username, string password)
        {
            return string.Equals(username, _expectedUsername, StringComparison.Ordinal) &&
                   string.Equals(password, _expectedPassword, StringComparison.Ordinal);
        }
    }
}
