using Microsoft.AspNetCore.Mvc;
using ReaderBuddy.WebApi.Models.DTOs;
using ReaderBuddy.WebApi.Services;

namespace ReaderBuddy.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookmarksController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;
    private readonly ILogger<BookmarksController> _logger;

    public BookmarksController(IBookmarkService bookmarkService, ILogger<BookmarksController> logger)
    {
        _bookmarkService = bookmarkService;
        _logger = logger;
    }

    /// <summary>
    /// Get all bookmarks
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookmarkDto>>> GetBookmarks()
    {
        try
        {
            var bookmarks = await _bookmarkService.GetAllBookmarksAsync();
            return Ok(bookmarks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookmarks");
            return StatusCode(500, "An error occurred while retrieving bookmarks");
        }
    }

    /// <summary>
    /// Get a specific bookmark by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<BookmarkDto>> GetBookmark(int id)
    {
        try
        {
            var bookmark = await _bookmarkService.GetBookmarkByIdAsync(id);
            if (bookmark == null)
            {
                return NotFound($"Bookmark with ID {id} not found");
            }
            return Ok(bookmark);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookmark with ID: {BookmarkId}", id);
            return StatusCode(500, "An error occurred while retrieving the bookmark");
        }
    }

    /// <summary>
    /// Create a new bookmark
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BookmarkDto>> CreateBookmark([FromBody] CreateBookmarkDto createBookmarkDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBookmark = await _bookmarkService.CreateBookmarkAsync(createBookmarkDto);
            return CreatedAtAction(nameof(GetBookmark), new { id = createdBookmark.Id }, createdBookmark);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bookmark");
            return StatusCode(500, "An error occurred while creating the bookmark");
        }
    }

    /// <summary>
    /// Update an existing bookmark
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBookmark(int id, [FromBody] CreateBookmarkDto updateBookmarkDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookmarkService.UpdateBookmarkAsync(id, updateBookmarkDto);
            return NoContent();
        }
        catch (ArgumentException)
        {
            return NotFound($"Bookmark with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bookmark with ID: {BookmarkId}", id);
            return StatusCode(500, "An error occurred while updating the bookmark");
        }
    }

    /// <summary>
    /// Delete a bookmark
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBookmark(int id)
    {
        try
        {
            await _bookmarkService.DeleteBookmarkAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bookmark with ID: {BookmarkId}", id);
            return StatusCode(500, "An error occurred while deleting the bookmark");
        }
    }

    /// <summary>
    /// Search bookmarks by title, description, or URL
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BookmarkDto>>> SearchBookmarks([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term is required");
            }

            var bookmarks = await _bookmarkService.SearchBookmarksAsync(searchTerm);
            return Ok(bookmarks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching bookmarks with term: {SearchTerm}", searchTerm);
            return StatusCode(500, "An error occurred while searching bookmarks");
        }
    }

    /// <summary>
    /// Get all available tags
    /// </summary>
    [HttpGet("tags")]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
    {
        try
        {
            var tags = await _bookmarkService.GetAllTagsAsync();
            return Ok(tags);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tags");
            return StatusCode(500, "An error occurred while retrieving tags");
        }
    }
}