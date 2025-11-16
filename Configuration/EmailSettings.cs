namespace Mosaic.Configuration;

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; }
    public string SmtpPassword { get; set; }
    public string SmtpSender { get; set; }
    public List<string> Subscribers { get; set; }
}
