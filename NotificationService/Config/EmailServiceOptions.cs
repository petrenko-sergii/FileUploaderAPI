namespace NotificationService.Config;

public class EmailServiceOptions
{
    public string? ConnectionString { get; set; }

    public string? SenderAddress { get; set; }

    public string? RecipientAddress { get; set; }
}
