using Microsoft.Extensions.Localization;
using System;
using Web.Interfaces;
using Web.Models;

namespace Web.Services
{
    public class EmailTemplate : IEmailTemplate
    {
        private string Template { get; set; }
        private string Subject { get; set; }
        private readonly IStringLocalizer<SharedResource> _localizer;

        public EmailTemplate(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public (string subject, string template) GetTemplate(EmailMessageType messageType, string callbackUrl)
        {
            switch (messageType)
            {
                case EmailMessageType.ForgotPasswordConfirmation:
                    Template = $"<body leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\" style=\"width: 100%; margin: 0; padding: 0;\">" +
                        "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" class=\"container\" style=\"border-collapse: collapse; width:100%; min-width: 100%; height: auto;\">" +
                          "<tr>" +
                            "<td width=\"100%\" valign=\"top\" style=\"padding-top:20px;\">" +
                             "<table width=\"580\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; margin: 0 auto;\">" +
                              "<tr>" +
                               "<td style=\"font-size:13px;font-weight:normal; text-align:left;font-family:'Open Sans', sans-serif;line-height:24px;vertical-align:top;padding:15px 8px 10px 8px;\">" +
                                "<h1 style=\"text-align:center;font-weight:600;margin:30px 0 50=px 0;\">Certificate</h1>" +
                                  " <a href=" + callbackUrl + " style =\"padding:10px;width:300px;display:block;text-decoration:none;border:1px;text-align:center;margin-left:21%; font-size:14px;color: white;background:#2478ff;border-radius:5px;line-height:17px;margin:0auto;\">" +
                                    _localizer["ForgotConfirmSend"] +
                                    "</a>" +
                                  "</td>" +
                                "</tr>" +
                              "</table>" +
                            "</td>" +
                          "</tr>" +
                        "</table>" + "</body>";
                    Subject = _localizer["ForgotEmailSend"];
                    return (Subject, Template);

                case EmailMessageType.RegisterConfirmation:
                    Template = $"<body leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\" style=\"width: 100%; margin: 0; padding: 0;\">" +
                        "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" class=\"container\" style=\"border-collapse: collapse; width:100%; min-width: 100%; height: auto;\">" +
                          "<tr>" +
                            "<td width=\"100%\" valign=\"top\" style=\"padding-top:20px;\">" +
                             "<table width=\"580\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; margin: 0 auto;\">" +
                              "<tr>" +
                               "<td style=\"font-size:13px;font-weight:normal; text-align:left;font-family:'Open Sans', sans-serif;line-height:24px;vertical-align:top;padding:15px 8px 10px 8px;\">" +
                                "<h1 style=\"text-align:center;font-weight:600;margin:30px 0 50=px 0;\">Certificate</h1>" +
                                  " <a href=" + callbackUrl + " style =\"padding:10px;width:300px;display:block;text-decoration:none;border:1px;text-align:center;margin-left:21%; font-size:14px;color: white;background:#2478ff;border-radius:5px;line-height:17px;margin:0auto;\">" +
                                    _localizer["ConfirmSend"] +
                                    "</a>" +
                                  "</td>" +
                                "</tr>" +
                              "</table>" +
                            "</td>" +
                          "</tr>" +
                        "</table>" + "</body>";
                    Subject = _localizer["ConfirmEmailSend"];
                    return (Subject, Template);

                default:
                    throw new ArgumentNullException();
            }
        }
    }
}
