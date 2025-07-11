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
    public async Task SendEmail(string fileInfo)
    {
        var emailClient = new EmailClient(_emailServiceOptions.ConnectionString);

        var emailMessage = new EmailMessage(
            senderAddress: _emailServiceOptions.SenderAddress,
            content: new EmailContent($"File \"{fileInfo}\" was uploaded")
            {
                Html = $"<strong>File \"{fileInfo}\" was uploaded.</strong>"
            },
            recipients: new EmailRecipients(
                [new EmailAddress(_emailServiceOptions.RecipientAddress)]));

        EmailSendOperation emailSendOperation = await emailClient.SendAsync(
            WaitUntil.Completed,
            emailMessage);
    }
}
