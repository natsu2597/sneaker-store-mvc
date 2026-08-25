namespace SneakerStore.Services.Services
{
    public interface IPasswordResetService
    {
        Task PasswordResetRequestAsync(string email, string baseUrl);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> ResetPasswordASync(string token, string newPassword);
    }
}
