namespace SneakerStore.Services.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetUrl);
    }
}
