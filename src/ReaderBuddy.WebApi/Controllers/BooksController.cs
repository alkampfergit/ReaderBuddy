using Microsoft.AspNetCore.Mvc;
using ReaderBuddy.WebApi.Models;
using ReaderBuddy.WebApi.Services;

namespace ReaderBuddy.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IBookService bookService, ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    /// <summary>
    /// Get all books
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        try
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving books");
            return StatusCode(500, "An error occurred while retrieving books");
        }
    }

    /// <summary>
    /// Get a specific book by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found");
            }
            return Ok(book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving book with ID: {BookId}", id);
            return StatusCode(500, "An error occurred while retrieving the book");
        }
    }

    /// <summary>
    /// Create a new book
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBook = await _bookService.CreateBookAsync(book);
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating book");
            return StatusCode(500, "An error occurred while creating the book");
        }
    }

    /// <summary>
    /// Update an existing book
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        try
        {
            if (id != book.Id)
            {
                return BadRequest("Book ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBook = await _bookService.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound($"Book with ID {id} not found");
            }

            await _bookService.UpdateBookAsync(book);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating book with ID: {BookId}", id);
            return StatusCode(500, "An error occurred while updating the book");
        }
    }

    /// <summary>
    /// Delete a book
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found");
            }

            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting book with ID: {BookId}", id);
            return StatusCode(500, "An error occurred while deleting the book");
        }
    }

    /// <summary>
    /// Search books by title, author, or genre
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Book>>> SearchBooks([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term is required");
            }

            var books = await _bookService.SearchBooksAsync(searchTerm);
            return Ok(books);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching books with term: {SearchTerm}", searchTerm);
            return StatusCode(500, "An error occurred while searching books");
        }
    }
}