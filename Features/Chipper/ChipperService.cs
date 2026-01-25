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

    public async Task<Response<bool>> Log(List<LogMessage> messages)
    {
        foreach (var m in messages)
        {
            await Log(m);
        }
        return Response<bool>.Success(true);
    }

    private async Task Log(LogMessage message)
    {
        await _chipperRepository.AddLogMessage(message);
        var subscribers = await _chipperRepository.GetAppSourceSubscribers(1, message.Level);
        foreach (var sub in subscribers)
        {
            await _emailRepository.SendEmailFromMosaicSender(new Email.Email()
            {
                To = new EmailAddress() { Address = sub.Email, DisplayName = string.Empty },
                Subject = $"(Investigations) - {message.RenderedMessage}",
                Body = ToEmailBodyString(message)
            });
        }
    }

    public async Task<Response<bool>> UpdateSubscription(AppSourceSubscriber subscriber)
    {
        await _chipperRepository.UpdateSubscription(subscriber);
        return Response<bool>.Success(true);
    }

    private string ToEmailBodyString(LogMessage message)
    {
        var builder = new StringBuilder();

        builder.Append($"<h1>(Investigations) - {message.Level}</h1>");
        builder.Append($"<p>{message.RenderedMessage}</p>");

        if (!string.IsNullOrEmpty(message.Exception))
        {
            builder.Append("<h2>Exception</h2>");
            builder.Append($"<p>{message.Exception}</p>");
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
