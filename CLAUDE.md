# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ReaderBuddy is a full-stack reading tracking application with:
- **Backend**: ASP.NET Core 9 Web API with Entity Framework Core and SQLite/SQL Server (`src/server/`)
- **Frontend**: React TypeScript application with TailwindCSS (`src/client/`)
- **Architecture**: Layered architecture with Repository pattern, services, and dependency injection
- **Database**: Entity Framework Code-First with automatic migrations (SQLite for dev, SQL Server for prod)
- **Testing**: xUnit with Moq for mocking, includes integration tests (`src/tests/`)
- **DevOps**: Docker containerization with multi-stage builds, GitHub Actions CI/CD

## Essential Commands

### .NET API Development
```bash
# Build and test (from repository root)
dotnet restore                    # Restore NuGet packages
dotnet build                      # Build solution
dotnet test                       # Run all tests with coverage
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage

# Run the API (automatically initializes database)
dotnet run --project src/server
# API available at: http://localhost:5201
# Swagger UI at: http://localhost:5201/swagger

# Entity Framework migrations
dotnet ef migrations add <MigrationName> --project src/server
dotnet ef database update --project src/server
dotnet ef migrations remove --project src/server
```

### React Client Development
```bash
# From src/client/ directory
npm install                       # Install dependencies
npm start                         # Start development server (http://localhost:3000)
npm run build                     # Build for production
npm test                          # Run React tests
```

### Docker Operations
```bash
# Development with hot reload
docker-compose -f docker-compose.dev.yml up

# Production build
docker-compose up

# Manual Docker build
docker build -t readerbuddy .
docker run -p 8080:8080 readerbuddy
```

### Testing Commands
```bash
# Run specific test projects
dotnet test src/tests/ReaderBuddy.WebApi.Tests/
dotnet test --filter "FullyQualifiedName~BookService"
dotnet test --verbosity normal
```

## Architecture & Key Patterns

### Backend Structure
- **Controllers**: Thin controllers handling HTTP concerns (`/Controllers/`)
- **Services**: Business logic layer with interfaces (`/Services/`)
- **Repository Pattern**: Generic repository with `IRepository<T>` interface (`/Data/`)
- **Models**: Domain entities and DTOs (`/Models/` and `/Models/DTOs/`)
- **Configuration**: Strongly-typed settings (`/Configuration/`)
- **DbContext**: Entity Framework context with relationships

### Key Technologies
- **.NET 9**: Latest framework with C# 13 features
- **Entity Framework Core 9.0.8**: ORM with Code-First migrations
- **Database**: SQLite for development (cross-platform), SQL Server for production
- **Swagger/OpenAPI**: API documentation with Swashbuckle
- **Dependency Injection**: Built-in ASP.NET Core DI container
- **xUnit**: Testing framework with Microsoft.AspNetCore.Mvc.Testing
- **Moq 4.20.72**: Mocking framework for unit tests

### Frontend Architecture
- **React 19.1.1**: Modern functional components with hooks
- **TypeScript**: Strict typing with `@types` packages
- **TailwindCSS 4.1.12**: Utility-first CSS framework
- **Axios**: HTTP client for API communication
- **React Testing Library**: Component testing with Jest

## Database & Entity Framework

### Connection Strings & Auto-Initialization
- **Development**: SQLite (`Data Source=ReaderBuddy_Dev.db`) - automatically created on startup
- **Production**: SQL Server - configure via `appsettings.Production.json`
- **Auto-Migration**: Database schema is automatically created/updated on application startup

### Key Entities
- **Book**: Core book entity with metadata
- **Bookmark**: User bookmarks with URLs and descriptions
- **Reading**: Reading sessions and progress tracking
- **Tag/BookmarkTag**: Many-to-many relationship for bookmark categorization

### Migration Workflow
1. Modify models in `src/server/Models/`
2. Add migration: `dotnet ef migrations add <Name> --project src/server`
3. Review generated migration in `src/server/Migrations/`
4. Database is automatically updated on application startup (no manual update needed)

## Git Flow & CI/CD

### Branch Strategy (GitVersion configured)
- **master**: Production releases, triggers Docker builds and git tags
- **develop**: Integration branch for features
- **feature/***: Feature development branches
- **release/***: Release preparation with beta versioning
- **hotfix/***: Critical fixes with hotfix versioning

### Semantic Versioning
- **Major**: `+semver: major` or `+semver: breaking`
- **Minor**: `+semver: minor` or `+semver: feature` 
- **Patch**: `+semver: patch` or `+semver: fix`
- **None**: `+semver: none` or `+semver: skip`

### CI/CD Pipeline Triggers
- **Automatic**: All branch pushes and PRs to master/develop
- **Docker Builds**: master and hotfix/* branches only
- **Git Tags**: Automatic on master branch pushes (format: `v{version}`)
- **Manual**: Workflow dispatch with optional Docker build override

## Development Guidelines

### Code Patterns to Follow
- **Repository Pattern**: Use `IRepository<T>` for data access
- **Service Layer**: Business logic in services with interfaces
- **Dependency Injection**: Register services in `Program.cs`
- **DTOs**: Use separate DTOs for API contracts vs domain models
- **Async/Await**: All database operations should be async
- **Logging**: Use `ILogger<T>` for structured logging
- **Error Handling**: Consistent exception handling in controllers

### API Conventions
- **RESTful Design**: Standard HTTP verbs and status codes
- **Route Patterns**: `api/[controller]` with attribute routing
- **Swagger Documentation**: XML comments for API documentation
- **CORS**: Configured for development (restrict in production)
- **Health Checks**: `/health` endpoint for monitoring

### Testing Approach
- **Unit Tests**: Service layer and business logic
- **Integration Tests**: Full HTTP pipeline testing
- **Mocking**: Use Moq for external dependencies
- **In-Memory Database**: Entity Framework InMemory provider for tests
- **Test Coverage**: Aim for comprehensive coverage with `XPlat Code Coverage`

## Configuration Management

### Application Settings Structure
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ReaderBuddy.db"
  },
  "ApplicationSettings": {
    "Name": "ReaderBuddy",
    "Version": "1.0.0",
    "EnableDetailedErrors": false
  }
}
```

### Environment-Specific Files
- `appsettings.json`: Base configuration
- `appsettings.Development.json`: Development overrides
- Create `appsettings.Production.json` as needed for production

## Docker & Deployment

### Multi-Stage Dockerfile
1. **Build Stage**: SDK image for compilation
2. **Publish Stage**: Creates deployment artifacts  
3. **Runtime Stage**: Minimal ASP.NET Core runtime image

### Docker Compose Configurations
- `docker-compose.yml`: Production setup
- `docker-compose.dev.yml`: Development with volume mounts
- Automatic service discovery and networking
- Environment variable configuration

## Troubleshooting Common Issues

### Entity Framework Issues
- **Migration Conflicts**: Use `dotnet ef migrations remove --project src/server` and recreate
- **Database Connection**: SQLite database is automatically created, no setup required
- **Cross-Platform**: SQLite works on all platforms (Windows, macOS, Linux)

### React Development Issues  
- **API Communication**: Check CORS configuration and API base URL
- **Build Errors**: Verify Node.js version compatibility (18+)
- **Dependency Issues**: Delete `node_modules` and run `npm install`

### Docker Issues
- **Build Context**: Ensure Dockerfile is at repository root
- **File Paths**: Use forward slashes in Docker for cross-platform compatibility
- **Multi-stage Caching**: Leverage Docker BuildKit for faster builds

## Development Workflow

### Starting Development
1. `git clone` and `cd ReaderBuddy`
2. `dotnet restore` - restore .NET packages
3. `cd src/client && npm install` - install React dependencies  
4. Run API: `dotnet run --project src/server` (database auto-initializes)
5. Run UI: `cd src/client && npm start`
6. Set REACT_APP_API_URL=http://localhost:5201/api for local development

### Making Changes
1. Create feature branch: `git checkout -b feature/my-feature`
2. Implement changes following architecture patterns
3. Add/update tests for new functionality
4. Run full test suite: `dotnet test`
5. Build and verify: `dotnet build && cd src/client && npm run build`
6. Commit with semantic versioning: `git commit -m "feat: add feature +semver: minor"`

### Before Merging
- Ensure all tests pass
- Verify API documentation is updated
- Check that migrations are included if database changes made
- Confirm Docker build works if infrastructure changes made