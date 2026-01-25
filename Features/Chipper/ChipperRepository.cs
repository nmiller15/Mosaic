using Mosaic.Shared;
using Microsoft.EntityFrameworkCore;

namespace Mosaic.Features.Chipper;

public class ChipperRepository : IChipperRepository
{
    private readonly MosaicDbContext _context;

    public ChipperRepository(MosaicDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddLogMessage(LogMessage message)
    {
        await _context.LogMessages.AddAsync(message);
        var rowsAffected = await _context.SaveChangesAsync();
        return rowsAffected > 0;
    }

    public async Task<List<AppSourceSubscriber>> GetAppSourceSubscribers(int appSourceKey, string logLevel)
    {
        var minLevel = GetLevelInt(logLevel);
        var subscribers = await _context.AppSourceSubscribers
            .Where(s => s.AppSourceKey == appSourceKey && s.Level >= minLevel)
            .ToListAsync();
        return subscribers;
    }

    public async Task UpdateSubscription(AppSourceSubscriber subscriber)
    {
        var existing = await _context.AppSourceSubscribers
            .FirstOrDefaultAsync(s => s.AppSourceKey == subscriber.AppSourceKey && s.Email == subscriber.Email);

        if (existing != null)
        {
            existing.Level = subscriber.Level;
            await _context.SaveChangesAsync();
        }
        else
        {
            await _context.AppSourceSubscribers.AddAsync(subscriber);
            await _context.SaveChangesAsync();
        }
    }

    private int GetLevelInt(string level)
    {
        return level.ToLower() switch
        {
            "debug" => 1,
            "info" => 2,
            "warning" => 3,
            "error" => 4,
            "critical" => 5,
            _ => throw new InvalidOperationException("Invalid log level"),
        };
    }

    private string GetLevelString(int priority)
    {
        return priority switch
        {
            1 => "debug",
            2 => "info",
            3 => "warning",
            4 => "error",
            5 => "critical",
            _ => throw new InvalidOperationException("Invalid log integer")
        };
    }
}
