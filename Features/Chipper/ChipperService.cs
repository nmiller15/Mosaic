using System.Text;
using Mosaic.Features.Email;
using Mosaic.Features.Logging;
using Mosaic.Shared;

namespace Mosaic.Features.Chipper;

public class ChipperService : IChipperService
{
    private readonly IChipperRepository _chipperRepository;
    private readonly IEmailRepository _emailRepository;

    public ChipperService(IChipperRepository chipperRepository, IEmailRepository emailRepository)
    {
        _chipperRepository = chipperRepository;
        _emailRepository = emailRepository;
    }

    public async Task<Response<bool>> Log(LogMessage message)
    {
        await _chipperRepository.AddLogMessage(message);
        Console.WriteLine($"Logged message from source {message.SourceKey} at level {message.Level}");
        var subscribers = await _chipperRepository.GetAppSourceSubscribers(message.SourceKey, message.Level);
        foreach (var sub in subscribers)
        {
            await _emailRepository.SendEmailFromMosaicSender(new Email.Email()
            {
                To = new EmailAddress() { Address = sub.Email, DisplayName = string.Empty },
                Subject = $"({message.SourceKey}) - {message.Message}",
                Body = ToEmailBodyString(message)
            });
        }

        return Response<bool>.Success(true);
    }

    public async Task<Response<bool>> UpdateSubscription(AppSourceSubscriber subscriber)
    {
        await _chipperRepository.UpdateSubscription(subscriber);
        return Response<bool>.Success(true);
    }

    private string ToEmailBodyString(LogMessage message)
    {
        var builder = new StringBuilder();

        builder.Append($"<h1>({message.SourceKey}) - {message.Level}</h1>");
        builder.Append($"<p>{message.Message}</p>");

        if (!string.IsNullOrEmpty(message.Environment))
        {
            builder.Append($"<p><strong>Environment:</strong> {message.Environment}</p>");
        }

        if (!string.IsNullOrEmpty(message.Exception))
        {
            builder.Append("<h2>Exception</h2>");
            builder.Append($"<p>{message.Exception}</p>");
        }

        if (!string.IsNullOrEmpty(message.StackTrace))
        {
            builder.Append("<h2>Stack Trace</h2>");
            builder.Append($"<pre>{message.StackTrace}</pre>");
        }

        if (message.Properties != null && message.Properties.Count > 0)
        {
            builder.Append("<h2>Properties</h2>");
            builder.Append("<ul>");
            foreach (var property in message.Properties)
            {
                builder.Append($"<li>{property.Key}: {property.Value}</li>");
            }
            builder.Append("</ul>");
        }

        return builder.ToString();
    }
}
