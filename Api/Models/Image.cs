namespace Models;

public class Image
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;

    public int? ToDontId { get; set; }
    public ToDont? ToDont { get; set; }
}