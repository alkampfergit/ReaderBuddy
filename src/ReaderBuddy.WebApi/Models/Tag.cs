namespace ReaderBuddy.WebApi.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#007bff"; // Default blue color
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<BookmarkTag> BookmarkTags { get; set; } = new List<BookmarkTag>();
}