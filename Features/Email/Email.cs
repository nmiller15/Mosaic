using System.Text;

namespace Mosaic.Features.Email;

public class Email
{
    public EmailAddress To { get; set; }
    public EmailAddress From { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public string ToString()
    {
        var builder = new StringBuilder();

        builder.AppendLine("<Email>");
        builder.AppendLine($"|  From: {From.DisplayName}: {From.Address}");
        builder.AppendLine($"|  To: {To.DisplayName}: {To.Address}");
        builder.AppendLine($"|  Subject: {Subject}");
        builder.AppendLine($"|  Body: {Body}");

        return builder.ToString();
    }
}
