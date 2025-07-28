using FileUploaderAPI.Server.Interfaces;
using FileUploaderAPI.Server.Models;
using Microsoft.AspNetCore.SignalR;

namespace FileUploaderAPI.Server.Helpers;

public class ProgressBarHelper : IProgressBarHelper
{
    private readonly IHubContext<UploadProgressHub> _hubContext;
    public ProgressBarHelper(IHubContext<UploadProgressHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendProgressBarData(Models.File file, long? totalBytes)
    {
        long totalBytesRead = 0;
        byte[] buffer = new byte[81920];
        int bytesRead;

        while ((bytesRead = await file.Stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            totalBytesRead += bytesRead;

            if (totalBytes.HasValue && totalBytes.Value > 0)
            {
                int progress = (int)(totalBytesRead * 100 / totalBytes.Value);
                await _hubContext.Clients.All.SendAsync("UploadProgress", 
                    new ProgressInfo { 
                        Progress = progress, 
                        TotalBytesRead = totalBytesRead
                    });
            }
        }
    }
}
