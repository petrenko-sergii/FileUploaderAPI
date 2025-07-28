namespace FileUploaderAPI.Server.Interfaces;

public interface IMultipartContentValidator
{
    Task<Models.File> ValidateAndExtractFileAsync(string? contentType, Stream body);
}