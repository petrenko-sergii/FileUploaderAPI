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
    public async Task SendEmail(FileInfo fileInfo)
    {
        var emailClient = new EmailClient(_emailServiceOptions.ConnectionString);

        var fileSizeInMB = Math.Round(fileInfo.Size / (1024.0 * 1024.0), 2);

        var emailMessage = new EmailMessage(
            senderAddress: _emailServiceOptions.SenderAddress,
            content: new EmailContent($"File \"{fileInfo.Name}\" was uploaded")
            {
                Html = $"<strong>File \"{fileInfo.Name}\" was uploaded." +
                    $"<br />" +
                    $"Size: {fileSizeInMB} MB." +
                    $"<br />" +
                    $"Uri: {fileInfo.Uri}.</strong>"
            },
            recipients: new EmailRecipients(
                [new EmailAddress(_emailServiceOptions.RecipientAddress)]));

        EmailSendOperation emailSendOperation = await emailClient.SendAsync(
            WaitUntil.Completed,
            emailMessage);
    }
}
