using FileService.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace FileService.Services;

public class NotifyService : INotifyService
{
    private readonly string _hubBaseUrl;

    public NotifyService(IConfiguration configuration)
    {
        _hubBaseUrl = configuration
            .GetSection("NotificationService")["BaseUrl"] ??  string.Empty;
    }

    public async Task NotifyFileUploadedAsync(string fileInfo)
    {
        var hubUrl = $"{_hubBaseUrl}/notificationHub";

        var connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

        await connection.StartAsync();
        await connection.InvokeAsync("NotifyFileUploaded", fileInfo);
        await connection.StopAsync();
    }
}
