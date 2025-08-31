using Microsoft.EntityFrameworkCore;
using ReaderBuddy.WebApi.Data;
using ReaderBuddy.WebApi.Models;
using ReaderBuddy.WebApi.Models.DTOs;

namespace ReaderBuddy.WebApi.Services;

public class BookmarkService : IBookmarkService
{
    private readonly IRepository<Bookmark> _bookmarkRepository;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IRepository<BookmarkTag> _bookmarkTagRepository;
    private readonly ILogger<BookmarkService> _logger;

    public BookmarkService(
        IRepository<Bookmark> bookmarkRepository,
        IRepository<Tag> tagRepository,
        IRepository<BookmarkTag> bookmarkTagRepository,
        ILogger<BookmarkService> logger)
    {
        _bookmarkRepository = bookmarkRepository;
        _tagRepository = tagRepository;
        _bookmarkTagRepository = bookmarkTagRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<BookmarkDto>> GetAllBookmarksAsync()
    {
        _logger.LogInformation("Retrieving all bookmarks");
        
        var bookmarks = await _bookmarkRepository.GetAllAsync();
        var result = new List<BookmarkDto>();
        
        foreach (var bookmark in bookmarks)
        {
            var bookmarkDto = await MapToBookmarkDto(bookmark);
            result.Add(bookmarkDto);
        }
        
        return result;
    }

    public async Task<BookmarkDto?> GetBookmarkByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving bookmark with ID: {BookmarkId}", id);
        
        var bookmark = await _bookmarkRepository.GetByIdAsync(id);
        if (bookmark == null)
            return null;
            
        return await MapToBookmarkDto(bookmark);
    }

    public async Task<BookmarkDto> CreateBookmarkAsync(CreateBookmarkDto createBookmarkDto)
    {
        _logger.LogInformation("Creating new bookmark: {BookmarkTitle}", createBookmarkDto.Title);
        
        var bookmark = new Bookmark
        {
            Title = createBookmarkDto.Title,
            Url = createBookmarkDto.Url,
            Description = createBookmarkDto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdBookmark = await _bookmarkRepository.AddAsync(bookmark);
        
        // Handle tags
        await AssignTagsToBookmark(createdBookmark.Id, createBookmarkDto.Tags);
        
        return await MapToBookmarkDto(createdBookmark);
    }

    public async Task UpdateBookmarkAsync(int id, CreateBookmarkDto updateBookmarkDto)
    {
        _logger.LogInformation("Updating bookmark with ID: {BookmarkId}", id);
        
        var bookmark = await _bookmarkRepository.GetByIdAsync(id);
        if (bookmark == null)
            throw new ArgumentException($"Bookmark with ID {id} not found");

        bookmark.Title = updateBookmarkDto.Title;
        bookmark.Url = updateBookmarkDto.Url;
        bookmark.Description = updateBookmarkDto.Description;
        bookmark.UpdatedAt = DateTime.UtcNow;

        await _bookmarkRepository.UpdateAsync(bookmark);
        
        // Update tags
        await AssignTagsToBookmark(id, updateBookmarkDto.Tags);
    }

    public async Task DeleteBookmarkAsync(int id)
    {
        _logger.LogInformation("Deleting bookmark with ID: {BookmarkId}", id);
        
        var bookmark = await _bookmarkRepository.GetByIdAsync(id);
        if (bookmark != null)
        {
            await _bookmarkRepository.DeleteAsync(bookmark);
        }
    }

    public async Task<IEnumerable<BookmarkDto>> SearchBookmarksAsync(string searchTerm)
    {
        _logger.LogInformation("Searching bookmarks with term: {SearchTerm}", searchTerm);
        
        var bookmarks = await _bookmarkRepository.FindAsync(b =>
            b.Title.Contains(searchTerm) ||
            b.Description.Contains(searchTerm) ||
            b.Url.Contains(searchTerm));
            
        var result = new List<BookmarkDto>();
        foreach (var bookmark in bookmarks)
        {
            var bookmarkDto = await MapToBookmarkDto(bookmark);
            result.Add(bookmarkDto);
        }
        
        return result;
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        _logger.LogInformation("Retrieving all tags");
        
        var tags = await _tagRepository.GetAllAsync();
        return tags.Select(t => new TagDto
        {
            Id = t.Id,
            Name = t.Name,
            Color = t.Color
        });
    }

    private async Task<BookmarkDto> MapToBookmarkDto(Bookmark bookmark)
    {
        var bookmarkTags = await _bookmarkTagRepository.FindAsync(bt => bt.BookmarkId == bookmark.Id);
        var tagIds = bookmarkTags.Select(bt => bt.TagId).ToList();
        
        var tags = new List<TagDto>();
        foreach (var tagId in tagIds)
        {
            var tag = await _tagRepository.GetByIdAsync(tagId);
            if (tag != null)
            {
                tags.Add(new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Color = tag.Color
                });
            }
        }

        return new BookmarkDto
        {
            Id = bookmark.Id,
            Title = bookmark.Title,
            Url = bookmark.Url,
            Description = bookmark.Description,
            CreatedAt = bookmark.CreatedAt,
            UpdatedAt = bookmark.UpdatedAt,
            Tags = tags
        };
    }

    private async Task AssignTagsToBookmark(int bookmarkId, List<string> tagNames)
    {
        // Remove existing bookmark-tag relationships
        var existingBookmarkTags = await _bookmarkTagRepository.FindAsync(bt => bt.BookmarkId == bookmarkId);
        foreach (var existingBt in existingBookmarkTags)
        {
            await _bookmarkTagRepository.DeleteAsync(existingBt);
        }

        // Create or find tags and assign to bookmark
        foreach (var tagName in tagNames)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                continue;

            var existingTag = (await _tagRepository.FindAsync(t => t.Name == tagName.Trim())).FirstOrDefault();
            
            Tag tag;
            if (existingTag == null)
            {
                // Create new tag
                tag = new Tag
                {
                    Name = tagName.Trim(),
                    Color = "#007bff", // Default color
                    CreatedAt = DateTime.UtcNow
                };
                tag = await _tagRepository.AddAsync(tag);
            }
            else
            {
                tag = existingTag;
            }

            // Create bookmark-tag relationship
            var bookmarkTag = new BookmarkTag
            {
                BookmarkId = bookmarkId,
                TagId = tag.Id
            };
            await _bookmarkTagRepository.AddAsync(bookmarkTag);
        }
    }
}