namespace FileService.Services.Interfaces
{
    public interface INotifyService
    {
        Task NotifyFileUploadedAsync(string fileInfo);
    }
}