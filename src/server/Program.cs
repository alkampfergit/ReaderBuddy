using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ReaderBuddy.WebApi.Configuration;
using ReaderBuddy.WebApi.Data;
using ReaderBuddy.WebApi.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Services.Configure<ApplicationSettings>(
    builder.Configuration.GetSection(ApplicationSettings.SectionName));

// Add services to the container
builder.Services.AddControllers();

// Add Entity Framework with environment-specific database providers
builder.Services.AddDbContext<ReaderBuddyDbContext>(options =>
{
    if (builder.Environment.IsEnvironment("Testing"))
    {
        // For testing - will be overridden by test configuration
        options.UseInMemoryDatabase("TestDatabase");
    }
    else if (builder.Environment.IsDevelopment())
    {
        // Use SQLite for development (cross-platform)
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=ReaderBuddy.db";
        options.UseSqlite(connectionString);
    }
    else
    {
        // Use SQL Server for production
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        options.UseSqlServer(connectionString);
    }
});

// Add repository pattern
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add business services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ReaderBuddy API",
        Version = "v1",
        Description = "A comprehensive reading tracking API"
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Initialize database automatically (skip for testing)
if (!app.Environment.IsEnvironment("Testing"))
{
    await InitializeDatabaseAsync(app);
}

// Check if the React build directory exists and log information
var staticFilesPath = Path.Combine(builder.Environment.ContentRootPath, "..", "client", "build");
var uiDir = new DirectoryInfo(staticFilesPath);
var fullUiPath = uiDir.FullName;
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("UI path: {UiPath}", fullUiPath);
if (!Directory.Exists(staticFilesPath))
{
    logger.LogWarning("UI path not found: {UiPath}. Static files will not be served.", fullUiPath);
}
else
{
    logger.LogInformation("UI path found: {UiPath}. Static files will be served.", fullUiPath);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReaderBuddy API v1");
        c.RoutePrefix = "swagger"; // Serve Swagger UI at /swagger instead of root
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors();

// Serve static files from the React build directory if it exists
if (Directory.Exists(staticFilesPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(staticFilesPath),
        RequestPath = ""
    });
}

// Fallback to index.html for SPA routing (only if React build exists)
if (Directory.Exists(staticFilesPath))
{
    app.Use(async (context, next) =>
    {
        if (context.Request.Path.Value != null && 
            !context.Request.Path.Value.StartsWith("/api") && 
            !context.Request.Path.Value.StartsWith("/swagger") &&
            !context.Request.Path.Value.StartsWith("/health") &&
            !Path.HasExtension(context.Request.Path.Value))
        {
            context.Request.Path = "/index.html";
        }
        await next();
    });
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
   .WithName("HealthCheck")
   .WithOpenApi();

app.Run();

// Database initialization method
static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Initializing database...");
        var context = services.GetRequiredService<ReaderBuddyDbContext>();
        
        // Ensure database is created and apply any pending migrations
        await context.Database.MigrateAsync();
        
        logger.LogInformation("Database initialized successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while initializing the database.");
        
        // In development, we might want to continue running even if DB init fails
        // In production, you might want to throw and stop the application
        if (app.Environment.IsDevelopment())
        {
            logger.LogWarning("Database initialization failed, but continuing in development mode.");
        }
        else
        {
            throw;
        }
    }
}

// Make the implicit Program class public for testing
public partial class Program { }
