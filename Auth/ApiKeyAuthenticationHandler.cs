using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Mosaic.Auth;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "ApiKey";
    private List<WhitelistedClient> _whitelist;

    public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<List<WhitelistedClient>> whitelist
            )
        : base(options, logger, encoder, clock)
    {
        _whitelist = whitelist.Value;

    }

    protected override object? Events { get => base.Events; set => base.Events = value; }

    protected override string ClaimsIssuer => base.ClaimsIssuer;

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        return base.ToString();
    }

    protected override Task<object> CreateEventsAsync()
    {
        return base.CreateEventsAsync();
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        const string headerName = "X-Api-Key";

        if (!Request.Headers.TryGetValue(headerName, out var providedKey))
            return Task.FromResult(AuthenticateResult.Fail("API Key was not provided."));

        var validKeys = _whitelist.Select(w => w.ApiKey).ToList();

        if (!validKeys.Contains(providedKey))
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key provided."));

        var claims = new[] { new Claim(ClaimTypes.Name, "ApiKeyUser") };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        return base.HandleChallengeAsync(properties);
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        return base.HandleForbiddenAsync(properties);
    }

    protected override Task InitializeEventsAsync()
    {
        return base.InitializeEventsAsync();
    }

    protected override Task InitializeHandlerAsync()
    {
        return base.InitializeHandlerAsync();
    }

    protected override string? ResolveTarget(string? scheme)
    {
        return base.ResolveTarget(scheme);
    }
}
