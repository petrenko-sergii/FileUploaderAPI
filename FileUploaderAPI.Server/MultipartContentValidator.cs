using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;

namespace FileUploaderAPI.Server;

public class MultipartContentValidator : IMultipartContentValidator
{
    public async Task<File> ValidateAndExtractFileAsync(string? contentType, Stream body)
    {
        if (!MultipartRequestHelper.IsMultipartContentType(contentType))
        {
            throw new Exception("Not a multipart request");
        }

        var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var reader = new MultipartReader(boundary, body);

        var section = await reader.ReadNextSectionAsync();

        if (section == null)
        {
            throw new Exception("No sections in multipart defined");
        }

        if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
        {
            throw new Exception("No content disposition in multipart defined");
        }

        var fileName = contentDisposition?.FileNameStar?.ToString();
        if (string.IsNullOrEmpty(fileName)) 
        { 
            fileName = contentDisposition?.FileName?.ToString();
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new Exception("No filename defined.");
        }

        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
        {
            fileName = fileName.Substring(1, fileName.Length - 2);
        }

        return new File
        {
            Stream = section.Body,
            Name = fileName
        };
    }
}
