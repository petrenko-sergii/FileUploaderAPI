using FileUploaderAPI.Server.Helpers;
using FileUploaderAPI.Server.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;

namespace FileUploaderAPI.Server.Validators;

public class MultipartContentValidator : IMultipartContentValidator
{
    public async Task<Models.File> ValidateAndExtractFileAsync(HttpRequest httpRequest) 
    { 
        if (!MultipartRequestHelper.IsMultipartContentType(httpRequest.ContentType))
        {
            throw new Exception("Not a multipart request");
        }

        var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(httpRequest.ContentType));
        var reader = new MultipartReader(boundary, httpRequest.Body);

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

        if (httpRequest.ContentLength == null || httpRequest.ContentLength ==0)
        {
            throw new Exception("File's size is zero.");
        }

        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
        {
            fileName = fileName.Substring(1, fileName.Length - 2);
        }

        return new Models.File
        {
            Stream = section.Body,
            Name = fileName,
            ContentLength = httpRequest.ContentLength.Value
        };
    }
}
