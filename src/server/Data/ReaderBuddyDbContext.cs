using Microsoft.EntityFrameworkCore;
using ReaderBuddy.WebApi.Models;

namespace ReaderBuddy.WebApi.Data;

public class ReaderBuddyDbContext : DbContext
{
    public ReaderBuddyDbContext(DbContextOptions<ReaderBuddyDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Reading> Readings { get; set; } = null!;
    public DbSet<Bookmark> Bookmarks { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<BookmarkTag> BookmarkTags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Book entity
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Author).IsRequired().HasMaxLength(300);
            entity.Property(e => e.ISBN).HasMaxLength(20);
            entity.Property(e => e.Genre).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.ISBN).IsUnique();
        });

        // Configure Reading entity
        modelBuilder.Entity<Reading>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Book)
                  .WithMany(b => b.Readings)
                  .HasForeignKey(e => e.BookId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Bookmark entity
        modelBuilder.Entity<Bookmark>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // Configure Tag entity
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Color).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure BookmarkTag many-to-many relationship
        modelBuilder.Entity<BookmarkTag>(entity =>
        {
            entity.HasKey(e => new { e.BookmarkId, e.TagId });

            entity.HasOne(e => e.Bookmark)
                  .WithMany(b => b.BookmarkTags)
                  .HasForeignKey(e => e.BookmarkId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tag)
                  .WithMany(t => t.BookmarkTags)
                  .HasForeignKey(e => e.TagId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}