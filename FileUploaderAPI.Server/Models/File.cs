namespace FileUploaderAPI.Server.Models;

public class File
{
    public Stream Stream { get; set; } = null!;

    public string Name { get; set; } = null!;
}
