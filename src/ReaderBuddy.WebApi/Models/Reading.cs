namespace ReaderBuddy.WebApi.Models;

public class Reading
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ReadingStatus Status { get; set; }
    public int CurrentPage { get; set; }
    public string? Notes { get; set; }
    public int? Rating { get; set; } // 1-5 stars
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public virtual Book Book { get; set; } = null!;
}

public enum ReadingStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    Abandoned = 3
}