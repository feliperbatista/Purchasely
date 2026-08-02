using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Purchasely.Application.Interfaces;

namespace Purchasely.Infrastructure.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(configuration["Email:From"]!));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            configuration["Email:Host"]!,
            int.Parse(configuration["Email:Port"]!),
            false,
            cancellationToken
        );

        var username = configuration["Email:Username"];
        var password = configuration["Email:Password"];
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            await client.AuthenticateAsync(username, password, cancellationToken);

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}