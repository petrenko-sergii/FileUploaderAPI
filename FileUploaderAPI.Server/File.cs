namespace FileUploaderAPI.Server;

public class File
{
    public Stream Stream { get; set; } = null!;

    public string Name { get; set; } = null!;
}
