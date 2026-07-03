using Assignment_2_BE.DTOs;
using Assignment_2_BE.Models;
using Assignment_2_BE.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assignment_2_BE.service
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<SystemAccount> _accountRepo;
        private readonly IConfiguration _configuration;

        public AuthService(IGenericRepository<SystemAccount> accountRepo, IConfiguration configuration)
        {
            _accountRepo = accountRepo;
            _configuration = configuration;
        }

        public string? Authenticate(LoginRequestDTO request)
        {
            SystemAccount? account = null;

            // 1. Check if it's the default Admin account from appsettings
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (request.Email == adminEmail && request.Password == adminPassword)
            {
                account = new SystemAccount
                {
                    AccountId = 0, // Admin doesn't have an ID in DB
                    AccountEmail = adminEmail,
                    AccountRole = 0 // Let's say 0 is Admin
                };
            }
            else
            {
                // 2. Check DB
                account = _accountRepo.GetAllQueryable().FirstOrDefault(a => a.AccountEmail == request.Email && a.AccountPassword == request.Password);
            }

            if (account == null) return null;

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyStr = _configuration["Jwt:Key"] ?? "ThisIsASuperSecretKeyForJWTAuthenticationInFUNewsManagementSystem123!";
            var key = Encoding.UTF8.GetBytes(keyStr);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                    new Claim(ClaimTypes.Email, account.AccountEmail ?? ""),
                    new Claim(ClaimTypes.Role, account.AccountRole?.ToString() ?? "")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
