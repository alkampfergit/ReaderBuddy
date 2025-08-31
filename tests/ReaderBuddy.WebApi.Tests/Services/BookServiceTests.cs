using Microsoft.Extensions.Logging;
using Moq;
using ReaderBuddy.WebApi.Data;
using ReaderBuddy.WebApi.Models;
using ReaderBuddy.WebApi.Services;

namespace ReaderBuddy.WebApi.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IRepository<Book>> _mockRepository;
    private readonly Mock<ILogger<BookService>> _mockLogger;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _mockRepository = new Mock<IRepository<Book>>();
        _mockLogger = new Mock<ILogger<BookService>>();
        _bookService = new BookService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllBooksAsync_ShouldReturnAllBooks()
    {
        // Arrange
        var expectedBooks = new List<Book>
        {
            new() { Id = 1, Title = "Test Book 1", Author = "Test Author 1" },
            new() { Id = 2, Title = "Test Book 2", Author = "Test Author 2" }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedBooks);

        // Act
        var result = await _bookService.GetAllBooksAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetBookByIdAsync_WithValidId_ShouldReturnBook()
    {
        // Arrange
        var bookId = 1;
        var expectedBook = new Book { Id = bookId, Title = "Test Book", Author = "Test Author" };
        _mockRepository.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(expectedBook);

        // Act
        var result = await _bookService.GetBookByIdAsync(bookId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bookId, result.Id);
        Assert.Equal("Test Book", result.Title);
        _mockRepository.Verify(r => r.GetByIdAsync(bookId), Times.Once);
    }

    [Fact]
    public async Task CreateBookAsync_ShouldCreateBookWithTimestamps()
    {
        // Arrange
        var newBook = new Book { Title = "New Book", Author = "New Author" };
        var createdBook = new Book { Id = 1, Title = "New Book", Author = "New Author", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Book>())).ReturnsAsync(createdBook);

        // Act
        var result = await _bookService.CreateBookAsync(newBook);

        // Assert
        Assert.NotNull(result);
        _mockRepository.Verify(r => r.AddAsync(It.Is<Book>(b => 
            b.Title == newBook.Title && 
            b.Author == newBook.Author &&
            b.CreatedAt != default &&
            b.UpdatedAt != default)), Times.Once);
    }
}