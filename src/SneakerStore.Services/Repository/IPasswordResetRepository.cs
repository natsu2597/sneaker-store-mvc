using SneakerStore.Services.Models;

namespace SneakerStore.Services.Repository
{
    public interface IPasswordResetRepository
    {
        Task CreateAsync(PasswordResetToken token);
        Task<PasswordResetToken?> GetTokenHashAsync(string tokenHash);
        Task MarkUsedAsync(int tokenId);
        Task InvalidatUserTokenAsync(int userId);
    }
}
