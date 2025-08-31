namespace ReaderBuddy.WebApi.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Reading> Readings { get; set; } = new List<Reading>();
}