using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace API.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration configuration;

    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(
        string email,
        string token)
    {
        var message = new MimeMessage();

        message.From.Add(
            new MailboxAddress(
                "Instagram Clone",
                configuration["EmailSettings:From"]!));

        message.To.Add(
            MailboxAddress.Parse(email));

        message.Subject = "Reset your password";

        var body = $"""
        <h2>Password Reset</h2>

        <p>You requested to reset your password.</p>

        <p>Your reset token is:</p>

        <h3>{token}</h3>

        <p>This token expires in 15 minutes.</p>

        <p>If you did not request this, you can ignore this email.</p>
        """;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            configuration["EmailSettings:Host"],
            int.Parse(configuration["EmailSettings:Port"]!),
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            configuration["EmailSettings:Username"],
            configuration["EmailSettings:Password"]);

        await smtp.SendAsync(message);

        await smtp.DisconnectAsync(true);
    }
}