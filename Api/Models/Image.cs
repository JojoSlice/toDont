using System.ComponentModel.DataAnnotations;

namespace Models;

public class Image
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string FileName { get; set; } = string.Empty;
    public int? ToDontId { get; set; }
    public ToDont? ToDont { get; set; }
}