namespace ReaderBuddy.WebApi.Models;

public class Bookmark
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<BookmarkTag> BookmarkTags { get; set; } = new List<BookmarkTag>();
}