namespace Mosaic.Configuration;

public class RateLimitSettings
{
    public int PermitLimit { get; set; } = 100;
    public int WindowSeconds { get; set; } = 60;
}
