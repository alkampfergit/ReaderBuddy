using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReaderBuddy.WebApi.Controllers;
using ReaderBuddy.WebApi.Models;
using ReaderBuddy.WebApi.Services;

namespace ReaderBuddy.WebApi.Tests.Controllers;

public class BooksControllerTests
{
    private readonly Mock<IBookService> _mockBookService;
    private readonly Mock<ILogger<BooksController>> _mockLogger;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _mockBookService = new Mock<IBookService>();
        _mockLogger = new Mock<ILogger<BooksController>>();
        _controller = new BooksController(_mockBookService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetBooks_ShouldReturnOkWithBooks()
    {
        // Arrange
        var expectedBooks = new List<Book>
        {
            new() { Id = 1, Title = "Test Book 1", Author = "Test Author 1" },
            new() { Id = 2, Title = "Test Book 2", Author = "Test Author 2" }
        };
        _mockBookService.Setup(s => s.GetAllBooksAsync()).ReturnsAsync(expectedBooks);

        // Act
        var result = await _controller.GetBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBooks = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        Assert.Equal(2, returnedBooks.Count());
    }

    [Fact]
    public async Task GetBook_WithValidId_ShouldReturnOkWithBook()
    {
        // Arrange
        var bookId = 1;
        var expectedBook = new Book { Id = bookId, Title = "Test Book", Author = "Test Author" };
        _mockBookService.Setup(s => s.GetBookByIdAsync(bookId)).ReturnsAsync(expectedBook);

        // Act
        var result = await _controller.GetBook(bookId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBook = Assert.IsType<Book>(okResult.Value);
        Assert.Equal(bookId, returnedBook.Id);
    }

    [Fact]
    public async Task GetBook_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var bookId = 999;
        _mockBookService.Setup(s => s.GetBookByIdAsync(bookId)).ReturnsAsync((Book?)null);

        // Act
        var result = await _controller.GetBook(bookId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateBook_WithValidBook_ShouldReturnCreatedResult()
    {
        // Arrange
        var newBook = new Book { Title = "New Book", Author = "New Author" };
        var createdBook = new Book { Id = 1, Title = "New Book", Author = "New Author" };
        _mockBookService.Setup(s => s.CreateBookAsync(newBook)).ReturnsAsync(createdBook);

        // Act
        var result = await _controller.CreateBook(newBook);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedBook = Assert.IsType<Book>(createdResult.Value);
        Assert.Equal(createdBook.Id, returnedBook.Id);
        Assert.Equal("GetBook", createdResult.ActionName);
    }

    [Fact]
    public async Task DeleteBook_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var bookId = 1;
        var existingBook = new Book { Id = bookId, Title = "Test Book", Author = "Test Author" };
        _mockBookService.Setup(s => s.GetBookByIdAsync(bookId)).ReturnsAsync(existingBook);

        // Act
        var result = await _controller.DeleteBook(bookId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockBookService.Verify(s => s.DeleteBookAsync(bookId), Times.Once);
    }

    [Fact]
    public async Task DeleteBook_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var bookId = 999;
        _mockBookService.Setup(s => s.GetBookByIdAsync(bookId)).ReturnsAsync((Book?)null);

        // Act
        var result = await _controller.DeleteBook(bookId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _mockBookService.Verify(s => s.DeleteBookAsync(It.IsAny<int>()), Times.Never);
    }
}