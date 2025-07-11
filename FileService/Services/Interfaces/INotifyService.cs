namespace FileService.Services.Interfaces
{
    public interface INotifyService
    {
        Task NotifyFileUploadedAsync(FileInfo fileInfo);
    }
}