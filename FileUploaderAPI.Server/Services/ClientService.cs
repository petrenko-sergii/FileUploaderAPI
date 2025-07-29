using FileUploaderAPI.Server.Interfaces;

namespace FileUploaderAPI.Server.Services;

public class ClientService : IClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task ForwardFileToFileServiceAsync(Models.File file)
    {
        var content = new StreamContent(file.Stream);
        content.Headers.Add("X-File-Name", file.Name);
        content.Headers.Add("X-File-Length", file.ContentLength.ToString());

        var client = _httpClientFactory.CreateClient("FileService");
        var response = await client.PostAsync(string.Empty, content);

        response.EnsureSuccessStatusCode();
    }
}
