using FileUploaderAPI.Server.Interfaces;

namespace FileUploaderAPI.Server.Services;

public class ClientService : IClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task ForwardFileToFileServiceAsync(Stream fileStream, string fileName)
    {
        var content = new StreamContent(fileStream);
        content.Headers.Add("X-File-Name", fileName);

        var client = _httpClientFactory.CreateClient("FileService");
        var response = await client.PostAsync(string.Empty, content);

        response.EnsureSuccessStatusCode();
    }
}
