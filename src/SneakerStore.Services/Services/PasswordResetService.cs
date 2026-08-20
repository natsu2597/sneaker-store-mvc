using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using SneakerStore.Services.Models;
using SneakerStore.Services.Repository;
using System.Security.Cryptography;
using System.Text;

namespace SneakerStore.Services.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetRepository _tokenRepository;
        private readonly IEmailService _emailService;
        private readonly PasswordHasher<User> _passwordHasher;

        public PasswordResetService(IUserRepository userRepository,
            IPasswordResetRepository tokenRepository,
            IEmailService emailService,
            PasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
        }

        public async Task PasswordResetRequestAsync(string email, string baseUrl)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return;

            await _tokenRepository.InvalidatUserTokenAsync(user.Id);

            var tokenBytes = RandomNumberGenerator.GetBytes(32);

            var token = Convert.ToBase64String(tokenBytes);

            var tokenHash = ComputeHash(token);

            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                TokenHash = tokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                IsUsed = false
            };

            await _tokenRepository.CreateAsync(resetToken);

            var resetUrl =
            $"{baseUrl}/Account/ResetPassword?token=" +
            Uri.EscapeDataString(token);

            await _emailService.SendPasswordResetEmailAsync(user.Email, resetUrl);
        }

        
        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var tokenHash = ComputeHash(token);

            var resetToken = await _tokenRepository.GetTokenHashAsync(tokenHash);

            if (resetToken == null)
                return false;

            if (resetToken.IsUsed)
                return false;

            if (resetToken.ExpiresAt <= DateTime.UtcNow)
                return false;

            return true;


        }

        public async Task<bool> ResetPasswordASync(string token, string newPassword)
        {
            if (!await ValidateTokenAsync(token))
                return false;

            var tokenHash = ComputeHash(token);

            var resetToken = await _tokenRepository.GetTokenHashAsync(tokenHash);

            if (resetToken == null)
                return false;

            var user = _userRepository.GetByIdAsync(resetToken.UserId);

            if (user == null)
                return false;

            var passwordHash = _passwordHasher.HashPassword(
                    new User
                    {
                        Id = user.Id
                    },
                    newPassword
                );

            await _userRepository.UpdatePasswordAsync(resetToken.Id,passwordHash);

            await _tokenRepository.MarkUsedAsync(resetToken.Id);

            await _tokenRepository.InvalidatUserTokenAsync(user.Id);

            return true;

        }


        private static string ComputeHash(string value)
        {
            var bytes = SHA256.HashData(
                    Encoding.UTF8.GetBytes(value)
                );

            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
