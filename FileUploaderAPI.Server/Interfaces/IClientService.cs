namespace FileUploaderAPI.Server.Interfaces;

public interface IClientService
{
    Task ForwardFileToFileServiceAsync(Models.File file);
}