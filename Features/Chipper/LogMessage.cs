using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mosaic.Features.Chipper;

public class LogMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int SourceKey { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Environment { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string Level { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    public string? Exception { get; set; }
    public string? StackTrace { get; set; }

    public IDictionary<string, string>? Properties { get; set; }

    // Navigation property to AppSource
    public AppSource? AppSource { get; set; }
}
