# Mosaic - Agent Guidelines

This file contains guidelines for AI agents working on the Mosaic ASP.NET Core Web API project.

## Project Overview

Mosaic is a .NET 10.0 ASP.NET Core Web API using clean architecture with feature-based organization. The project integrates with PostgreSQL, email services (Brevo/MailKit), and external APIs with custom API key authentication.

## Build & Development Commands

```bash
# Build
dotnet build

# Run development server
dotnet run

# Run with hot reload
dotnet watch

# Production build (self-contained Linux)
dotnet publish -c Release -r linux-x64 --self-contained true -o ./publish
```

## Testing

**Note**: This project currently has no testing framework configured. If adding tests:

```bash
# Create test projects
dotnet new xunit -n Mosaic.Tests
dotnet new xunit -n Mosaic.IntegrationTests

# Required test packages
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Moq
```

## Code Style Guidelines

### C# Conventions
- **Naming**: PascalCase for classes, methods, properties; camelCase for locals/parameters
- **Interfaces**: Prefix with 'I' (e.g., `IEmailService`)
- **Private fields**: Prefix with underscore (e.g., `_emailRepository`)
- **Files**: One class per file, name matches class name
- **Braces**: Allman style (opening brace on new line)

### Using Statements
```csharp
// System namespaces first (alphabetical)
using System;
using System.Text;

// Then custom namespaces (alphabetical)
using Mosaic.Features.Email;
using Mosaic.Shared;
```

### Nullable Reference Types
- Enabled project-wide (`<Nullable>enable</Nullable>`)
- Use appropriate null annotations (`?`, `!`, `NotNullWhen`)

## Architecture Patterns

### Directory Structure
```
Mosaic/
├── Auth/                    # Authentication components
├── Configuration/           # DI and configuration setup
├── Features/               # Feature-based modules
│   ├── Brevo/
│   ├── Chipper/
│   ├── Database/
│   ├── Email/
│   ├── Logging/
│   └── Things/
├── Shared/                 # Shared models and utilities
└── Program.cs             # Application entry point
```

### Feature Organization
- Each feature has its own folder under `Features/`
- Keep related functionality together
- Use extension methods for DI setup in `Configuration/`

## Response Patterns

### Standard Response Wrapper
```csharp
public class Response<T>
{
    public bool WasSuccessful { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    
    public static Response<T> Success(T data) => new() { WasSuccessful = true, Data = data };
    public static Response<T> Failure(string message) => new() { WasSuccessful = false, Message = message };
}
```

### Error Handling
- Use global exception handling middleware
- Return appropriate HTTP status codes
- Log exceptions via `ILoggingService`
- Validate inputs and return meaningful error messages

## Dependency Injection Patterns

### Service Registration
```csharp
// In Configuration/ConfigureDependencyInjectionExtensions.cs
public static IServiceCollection AddFeatureServices(this IServiceCollection services, IConfiguration configuration)
{
    // Singleton for repositories/core services
    services.AddSingleton<IEmailRepository, EmailRepository>();
    
    // Scoped for feature services
    services.AddScoped<IEmailService, EmailService>();
    
    // HttpClient for external APIs
    services.AddHttpClient<IBrevoService, BrevoService>();
    
    return services;
}
```

### Lifetime Guidelines
- **Singleton**: Repositories, core services, configuration
- **Scoped**: Feature services, request-specific services
- **Transient**: Lightweight services, stateless utilities

## Configuration Management

### Settings Classes
```csharp
public class EmailSettings
{
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
```

### Configuration Extensions
```csharp
public static IServiceCollection ConfigureEmail(this IServiceCollection services, IConfiguration configuration)
{
    services.Configure<EmailSettings>(configuration.GetSection("Email"));
    return services;
}
```

## API Design Patterns

### Endpoint Registration
```csharp
// In Configuration/EndpointConfig.cs
public static void ConfigureFeatureEndpoints(this WebApplication app)
{
    app.MapGet("/api/feature/{id}", async (int id, IFeatureService service) =>
    {
        var result = await service.GetByIdAsync(id);
        return result.WasSuccessful ? Results.Ok(result) : Results.BadRequest(result);
    });
}
```

### Authentication
- Custom API key authentication via `X-Api-Key` header
- Use `WhitelistedClient` for client validation
- Claims-based authentication for authorized endpoints

## Database Patterns

### Repository Pattern
```csharp
public interface IEmailRepository
{
    Task<IEnumerable<Email>> GetAllAsync();
    Task<Email> GetByIdAsync(int id);
    Task<Email> CreateAsync(Email email);
    Task<bool> UpdateAsync(Email email);
    Task<bool> DeleteAsync(int id);
}
```

### Connection Management
- Use Npgsql for PostgreSQL
- Connection string in appsettings.json
- Dependency injection for `NpgsqlConnection`

## External API Integration

### HttpClient Usage
```csharp
public class BrevoService : IBrevoService
{
    private readonly HttpClient _httpClient;
    private readonly BrevoSettings _settings;
    
    public BrevoService(HttpClient httpClient, IOptions<BrevoSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _httpClient.BaseAddress = new Uri(_settings.ApiUrl);
    }
}
```

### Error Handling for External APIs
- Wrap external API calls in try-catch blocks
- Log failures appropriately
- Return meaningful error responses
- Implement retry logic where appropriate

## Email Integration

### MailKit Usage
```csharp
public class EmailService : IEmailService
{
    public async Task<Response<bool>> SendEmailAsync(EmailMessage message)
    {
        try
        {
            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, useSsl: true);
            await smtpClient.AuthenticateAsync(_settings.Username, _settings.Password);
            
            var emailMessage = new MimeMessage();
            // ... build message
            
            await smtpClient.SendAsync(emailMessage);
            await smtpClient.DisconnectAsync(true);
            
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email");
            return Response<bool>.Failure("Email sending failed");
        }
    }
}
```

## Logging Patterns

### Structured Logging
```csharp
_logger.LogInformation("Processing email request for {EmailAddress}", emailAddress);
_logger.LogError(ex, "Failed to process email for {EmailAddress}", emailAddress);
```

### Log Levels
- **Trace**: Detailed debugging information
- **Debug**: Development debugging information
- **Information**: General operational information
- **Warning**: Unexpected behavior that doesn't stop execution
- **Error**: Errors that prevent successful operation
- **Critical**: Serious failures requiring immediate attention

## Security Guidelines

### API Key Management
- Store API keys in configuration files (not in code)
- Use environment variables for production
- Implement proper key rotation procedures
- Validate API keys against whitelist

### Input Validation
- Validate all user inputs
- Use regular expressions for email validation
- Sanitize data before database operations
- Implement proper error messages without exposing sensitive information

## Performance Guidelines

### Async/Await Usage
- Use async/await for all I/O operations
- Avoid `.Result` or `.Wait()` on async calls
- Use `ConfigureAwait(false)` in library code
- Implement proper cancellation token usage

### Memory Management
- Use `using` statements for disposable objects
- Avoid memory leaks in long-running operations
- Implement proper object pooling for frequently created objects

## Deployment Notes

### Production Deployment
- Target: `linux-x64` self-contained deployment
- Output directory: `./publish`
- Configuration: Environment-specific appsettings
- No Docker containerization (direct deployment)

### Environment Configuration
- Development: `appsettings.Development.json`
- Production: Environment variables and `appsettings.Production.json`
- Custom settings: `mosaic.settings.{environment}.json`