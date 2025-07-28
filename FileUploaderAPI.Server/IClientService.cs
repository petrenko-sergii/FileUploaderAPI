namespace FileUploaderAPI.Server;

public interface IClientService
{
    Task ForwardFileToFileServiceAsync(Stream fileStream, string fileName);
}