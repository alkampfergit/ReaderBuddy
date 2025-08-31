namespace ReaderBuddy.WebApi.Models;

public class BookmarkTag
{
    public int BookmarkId { get; set; }
    public int TagId { get; set; }

    // Navigation properties
    public virtual Bookmark Bookmark { get; set; } = null!;
    public virtual Tag Tag { get; set; } = null!;
}