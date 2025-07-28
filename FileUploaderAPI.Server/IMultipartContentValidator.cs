namespace FileUploaderAPI.Server;

public interface IMultipartContentValidator
{
    Task<File> ValidateAndExtractFileAsync(string? contentType, Stream body);
}