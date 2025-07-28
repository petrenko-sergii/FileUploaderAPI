namespace FileUploaderAPI.Server;

public interface IProgressBarHelper
{
    Task SendProgressBarData(File file, long? totalBytes);
}