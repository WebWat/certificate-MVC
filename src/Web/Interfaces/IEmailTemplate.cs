using Web.Models;

namespace Web.Interfaces
{
    public interface IEmailTemplate
    {
        (string subject, string template) GetTemplate(EmailMessageType messageType, string callbackUrl);
    }
}
