using Microsoft.AspNetCore.SignalR;
using NotificationService.Services.Interfaces;

namespace NotificationService;

public class NotificationHub : Hub
{
    private readonly IEmailService _emailService;

    public NotificationHub(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task NotifyFileUploaded(FileInfo fileInfo)
    {
        await _emailService.SendEmail(fileInfo);
    }
}
