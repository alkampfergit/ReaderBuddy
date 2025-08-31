using ReaderBuddy.WebApi.Data;
using ReaderBuddy.WebApi.Models;

namespace ReaderBuddy.WebApi.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book> _bookRepository;
    private readonly ILogger<BookService> _logger;

    public BookService(IRepository<Book> bookRepository, ILogger<BookService> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        _logger.LogInformation("Retrieving all books");
        return await _bookRepository.GetAllAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving book with ID: {BookId}", id);
        return await _bookRepository.GetByIdAsync(id);
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        _logger.LogInformation("Creating new book: {BookTitle}", book.Title);
        book.CreatedAt = DateTime.UtcNow;
        book.UpdatedAt = DateTime.UtcNow;
        return await _bookRepository.AddAsync(book);
    }

    public async Task UpdateBookAsync(Book book)
    {
        _logger.LogInformation("Updating book with ID: {BookId}", book.Id);
        book.UpdatedAt = DateTime.UtcNow;
        await _bookRepository.UpdateAsync(book);
    }

    public async Task DeleteBookAsync(int id)
    {
        _logger.LogInformation("Deleting book with ID: {BookId}", id);
        var book = await _bookRepository.GetByIdAsync(id);
        if (book != null)
        {
            await _bookRepository.DeleteAsync(book);
        }
    }

    public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
    {
        _logger.LogInformation("Searching books with term: {SearchTerm}", searchTerm);
        return await _bookRepository.FindAsync(b =>
            b.Title.Contains(searchTerm) ||
            b.Author.Contains(searchTerm) ||
            b.Genre.Contains(searchTerm));
    }
}