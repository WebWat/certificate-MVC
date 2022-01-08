using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly Email _email;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<Email> options, ILogger<EmailSender> logger)
    {
        _email = options.Value;
        _logger = logger;
    }


    public async Task SendEmailAsync(string email, string subject, string template)
    {
        // Set all the necessary data to send.
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_email.Name, _email.Name));
        emailMessage.To.Add(new MailboxAddress(string.Empty, email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = template
        };

        // Sending an email.
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_email.Host, 465, true);
            await client.AuthenticateAsync(_email.Name, _email.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }

        _logger.LogInformation("The email was sent to " + email);
    }
}
