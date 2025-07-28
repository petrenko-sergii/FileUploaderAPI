namespace FileUploaderAPI.Server.Interfaces;

public interface IClientService
{
    Task ForwardFileToFileServiceAsync(Stream fileStream, string fileName);
}