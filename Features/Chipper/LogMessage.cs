using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mosaic.Features.Chipper;

public class LogMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    [Required]
    [MaxLength(20)]
    public string Level { get; set; } = string.Empty;

    public string MessageTemplate { get; set; } = string.Empty;

    public string RenderedMessage { get; set; } = string.Empty;

    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

    public string Exception { get; set; } = string.Empty;
}
