namespace FileService.Services.Interfaces;

public interface IHeadersHelper
{
    long GetFileLength(IHeaderDictionary headers);
    string GetFileName(IHeaderDictionary headers);
}