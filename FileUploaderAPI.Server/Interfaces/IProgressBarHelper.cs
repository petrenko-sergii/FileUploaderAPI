namespace FileUploaderAPI.Server.Interfaces;

public interface IProgressBarHelper
{
    Task SendProgressBarData(Models.File file);
}