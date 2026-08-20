using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SneakerStore.Services.Settings;

namespace SneakerStore.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetUrl)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                        _settings.FromName,
                        _settings.FromEmail
                    )
            );

            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Reset your password";

            var body = $"""
                    <html>
                    <body>

                        <h2>Reset you Password</h2>

                        <p>
                            We Received a request to reset your password
                        </p>

                        <p>
                            <a href="{resetUrl}">
                                Reset Password
                            </a>
                        </p>

                        <p>
                            This link expires in 30 minutes.
                        </p>

                        <p>
                            If you did not request this,
                            you can safely ignore this email.
                        </p>

                        <p>
                            — SneakerStore
                        </p>
                    </body>
                    </html>
                """
                ;

            message.Body = new BodyBuilder
            {
                HtmlBody = body
            }.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                    _settings.Host,
                    _settings.Port,
                    SecureSocketOptions.StartTls
                );

            await smtp.AuthenticateAsync(
                    _settings.Username,
                    _settings.Password
                );

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
                
        }
    }
}
