using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReaderBuddy.WebApi.Data;
using System.Net;
using System.Net.Http.Json;
using ReaderBuddy.WebApi.Models;

namespace ReaderBuddy.WebApi.Tests.Integration;

public class BooksIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BooksIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                // Remove the app DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ReaderBuddyDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Also remove the generic DbContext registration
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ReaderBuddyDbContext));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                // Add a database context using an in-memory database for testing
                services.AddDbContext<ReaderBuddyDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ReaderBuddyDbContext>();

                // Ensure the database is created
                db.Database.EnsureCreated();
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_Health_ReturnsHealthyStatus()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Healthy", content);
    }

    [Fact]
    public async Task Get_Books_ReturnsEmptyListInitially()
    {
        // Act
        var response = await _client.GetAsync("/api/books");

        // Assert
        response.EnsureSuccessStatusCode();
        var books = await response.Content.ReadFromJsonAsync<List<Book>>();
        Assert.NotNull(books);
        Assert.Empty(books);
    }

    [Fact]
    public async Task Post_Book_CreatesNewBook()
    {
        // Arrange
        var newBook = new Book
        {
            Title = "Test Book",
            Author = "Test Author",
            ISBN = "1234567890",
            Genre = "Fiction",
            Description = "A test book",
            PageCount = 300
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", newBook);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdBook = await response.Content.ReadFromJsonAsync<Book>();
        Assert.NotNull(createdBook);
        Assert.Equal(newBook.Title, createdBook.Title);
        Assert.Equal(newBook.Author, createdBook.Author);
        Assert.True(createdBook.Id > 0);
    }

    [Fact]
    public async Task Get_BookById_WithValidId_ReturnsBook()
    {
        // Arrange - Create a book first
        var newBook = new Book
        {
            Title = "Integration Test Book",
            Author = "Integration Test Author",
            ISBN = "9876543210",
            Genre = "Non-Fiction",
            Description = "An integration test book",
            PageCount = 250
        };

        var createResponse = await _client.PostAsJsonAsync("/api/books", newBook);
        var createdBook = await createResponse.Content.ReadFromJsonAsync<Book>();

        // Act
        var response = await _client.GetAsync($"/api/books/{createdBook!.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var returnedBook = await response.Content.ReadFromJsonAsync<Book>();
        Assert.NotNull(returnedBook);
        Assert.Equal(createdBook.Id, returnedBook.Id);
        Assert.Equal(newBook.Title, returnedBook.Title);
    }

    [Fact]
    public async Task Get_BookById_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/books/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}