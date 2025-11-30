using MimeKit;

namespace Mosaic.Features.Email;

public static class EmailExtensions
{
    public static MimeMessage ToMimeMessage(this Email email)
    {
        var message = new MimeMessage()
        {
            Subject = email.Subject,
            Body = new BodyBuilder() { HtmlBody = email.Body, }.ToMessageBody(),
        };

        message.From.Add(new MailboxAddress(email.From.DisplayName, email.From.Address));
        message.To.Add(new MailboxAddress(email.To.DisplayName, email.To.Address));

        return message;
    }
}
