using Azure.Communication.Email;
using Azure;
using NotificationService.Services.Interfaces;
using Microsoft.Extensions.Options;
using NotificationService.Config;

namespace NotificationService.Services;

public class EmailService : IEmailService
{
    private readonly EmailServiceOptions _emailServiceOptions;

    public EmailService(IOptions<EmailServiceOptions> emailServiceOptions)
    {
        _emailServiceOptions = emailServiceOptions.Value;
    }
    public void SendEmail(string fileInfo)
    {
        var emailClient = new EmailClient(_emailServiceOptions.ConnectionString);

        var emailMessage = new EmailMessage(
          senderAddress: _emailServiceOptions.SenderAddress,
          content: new EmailContent("Test Email")
          {
              PlainText = "Hello world via email.",
              Html = "<strong>File was uploaded</strong>"
          },
          recipients: new EmailRecipients(
              [new EmailAddress(_emailServiceOptions.RecipientAddress)]));

        EmailSendOperation emailSendOperation = emailClient.Send(
            WaitUntil.Completed,
            emailMessage);
    }
}
