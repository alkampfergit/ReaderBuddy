using ReaderBuddy.WebApi.Models;
using ReaderBuddy.WebApi.Models.DTOs;

namespace ReaderBuddy.WebApi.Services;

public interface IBookmarkService
{
    Task<IEnumerable<BookmarkDto>> GetAllBookmarksAsync();
    Task<BookmarkDto?> GetBookmarkByIdAsync(int id);
    Task<BookmarkDto> CreateBookmarkAsync(CreateBookmarkDto createBookmarkDto);
    Task UpdateBookmarkAsync(int id, CreateBookmarkDto updateBookmarkDto);
    Task DeleteBookmarkAsync(int id);
    Task<IEnumerable<BookmarkDto>> SearchBookmarksAsync(string searchTerm);
    Task<IEnumerable<TagDto>> GetAllTagsAsync();
}