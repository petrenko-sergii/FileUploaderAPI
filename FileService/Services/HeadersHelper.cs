using FileService.Services.Interfaces;

namespace FileService.Services;

public class HeadersHelper : IHeadersHelper
{
    public string GetFileName(IHeaderDictionary headers)
    {
        var fileName = headers["X-File-Name"].FirstOrDefault();
        if (string.IsNullOrEmpty(fileName))
        {
            throw new BadHttpRequestException("Missing X-File-Name header.");
        }

        return fileName;
    }

    public long GetFileLength(IHeaderDictionary headers)
    {
        long.TryParse(headers["X-File-Length"].FirstOrDefault(), out long fileLength);
        if (fileLength == 0)
        {
            throw new BadHttpRequestException("Header X-File-Length is missing or invalid.");
        }

        return fileLength;
    }
}
