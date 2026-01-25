using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mosaic.Features.Chipper;

public class AppSourceSubscriber
{
    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int AppSourceKey { get; set; }
    
    [Key]
    [Column(Order = 1)]
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    public int Level { get; set; }
}
