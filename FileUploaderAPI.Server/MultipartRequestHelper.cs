namespace FileUploaderAPI.Server;

public static class MultipartRequestHelper
{
    public static bool IsMultipartContentType(string contentType)
    {
        return !string.IsNullOrEmpty(contentType)
               && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static string GetBoundary(System.Net.Http.Headers.MediaTypeHeaderValue mediaTypeHeaderValue)
    {
        if (mediaTypeHeaderValue == null)
        {
            throw new ArgumentNullException(nameof(mediaTypeHeaderValue));
        }

        var boundary = mediaTypeHeaderValue.Parameters
            .FirstOrDefault(p => p.Name.Equals("boundary", StringComparison.OrdinalIgnoreCase))?.Value;

        if (string.IsNullOrWhiteSpace(boundary))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        return boundary;
    }
}
