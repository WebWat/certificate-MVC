using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Sends message to email
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly Email _email;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<Email> options, ILogger<EmailSender> logger)
        {
            _email = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string callbackUrl, string action)
        {
            string template = $"<body leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\" style=\"width: 100%; margin: 0; padding: 0;\">" +
            "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" class=\"container\" style=\"border-collapse: collapse; width:100%; min-width: 100%; height: auto;\">" +
              "<tr>" +
                "<td width=\"100%\" valign=\"top\" style=\"padding-top:20px;\">" +
                 "<table width=\"580\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; margin: 0 auto;\">" +
                  "<tr>" +
                   "<td style=\"font-size:13px;font-weight:normal; text-align:left;font-family:'Open Sans', sans-serif;line-height:24px;vertical-align:top;padding:15px 8px 10px 8px;\">" +
                    "<h1 style=\"text-align:center;font-weight:600;margin:30px 0 50=px 0;\">Certificate</h1>" +
                      " <a href=" + callbackUrl + " style =\"padding:10px;width:300px;display:block;text-decoration:none;border:1px;text-align:center;margin-left:21%; font-size:14px;color: white;background:#2478ff;border-radius:5px;line-height:17px;margin:0auto;\">" +
                       action +
                        "</a>" +
                      "</td>" +
                    "</tr>" +
                  "</table>" +
                "</td>" +
              "</tr>" +
            "</table>" + "</body>";

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_email.Name, _email.Name));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = template
            };

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
}
