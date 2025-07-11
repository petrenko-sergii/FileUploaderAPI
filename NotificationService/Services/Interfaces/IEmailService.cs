namespace NotificationService.Services.Interfaces;

public interface IEmailService
{
    Task SendEmail(FileInfo fileInfo);
}
