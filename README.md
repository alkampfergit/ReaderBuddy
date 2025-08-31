# ReaderBuddy

A comprehensive reading tracking API with React UI built with ASP.NET Core and Entity Framework Core.

## Project Structure

```
ReaderBuddy/
├── src/
│   ├── server/                       # ASP.NET Core Web API project
│   │   ├── Controllers/              # API controllers
│   │   ├── Models/                   # Domain models
│   │   │   └── DTOs/                 # Data Transfer Objects
│   │   ├── Services/                 # Business logic layer
│   │   ├── Data/                     # Data access layer
│   │   ├── Configuration/            # Configuration classes
│   │   ├── Migrations/               # Entity Framework migrations
│   │   └── Program.cs                # Application entry point
│   ├── client/                       # React frontend application
│   │   ├── src/
│   │   │   ├── components/           # React components
│   │   │   ├── services/             # API client services
│   │   │   ├── types/                # TypeScript type definitions
│   │   │   └── App.tsx               # Main App component
│   │   ├── public/                   # Static assets
│   │   └── package.json              # NPM dependencies
│   └── tests/
│       └── ReaderBuddy.WebApi.Tests/ # Test project
│           ├── Controllers/          # Controller tests
│           ├── Services/             # Service tests
│           └── Integration/          # Integration tests
├── Dockerfile                        # Docker configuration
├── docker-compose.yml               # Production Docker Compose
├── docker-compose.dev.yml           # Development Docker Compose
└── ReaderBuddy.sln                  # Solution file
```

## Features

- **RESTful API Design**: Follows REST principles with proper HTTP verbs and status codes
- **React Frontend UI**: Modern, responsive web interface for bookmark management
- **Bookmark Management**: Save, organize, and search bookmarks with tags
- **Layered Architecture**: Clear separation between controllers, services, and data access
- **Entity Framework Core**: Code-first approach with automatic database initialization
- **Repository Pattern**: Abstracted data access with generic repository implementation
- **Dependency Injection**: Built-in ASP.NET Core DI container
- **Configuration Management**: Environment-specific settings with strongly-typed configuration
- **Swagger/OpenAPI**: Interactive API documentation
- **Docker Support**: Containerization for development and production
- **Comprehensive Testing**: Unit tests, integration tests, and mocking examples
- **CI/CD Pipeline**: GitHub Actions with semantic versioning and automated Docker builds
- **Git Flow Support**: Structured branching model with automatic version management

## Technology Stack

- **.NET 9**: Latest LTS framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core**: Object-relational mapping
- **SQLite/SQL Server**: SQLite for development, SQL Server for production
- **xUnit**: Testing framework
- **Moq**: Mocking framework
- **Swagger/OpenAPI**: API documentation
- **Docker**: Containerization

## Getting Started

### Prerequisites

- .NET 9 SDK
- Node.js 18+ (for React client)
- Docker (optional)

*Note: No database setup required - SQLite is automatically initialized*

### Running Locally

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ReaderBuddy
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the API** (automatically initializes database)
   ```bash
   dotnet run --project src/server
   ```

4. **Access the API**
   - Swagger UI: http://localhost:5201/swagger
   - Health Check: http://localhost:5201/health
   - Database: SQLite file automatically created at `src/server/ReaderBuddy_Dev.db`

### Running the React UI

1. **Navigate to the client directory**
   ```bash
   cd src/client
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Configure API endpoint** (for local development)
   ```bash
   # Set environment variable for local development
   export REACT_APP_API_URL=http://localhost:5201/api
   ```

4. **Start the development server**
   ```bash
   npm start
   ```

5. **Access the application**
   - Web UI: http://localhost:3000
   - The React app will automatically reload when you make changes

6. **Build for production**
   ```bash
   npm run build
   ```

### Running with Docker

1. **Development environment**
   ```bash
   docker-compose -f docker-compose.dev.yml up
   ```

2. **Production environment**
   ```bash
   docker-compose up
   ```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test src/tests/ReaderBuddy.WebApi.Tests/

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## API Endpoints

### Books

- `GET /api/books` - Get all books
- `GET /api/books/{id}` - Get book by ID
- `POST /api/books` - Create new book
- `PUT /api/books/{id}` - Update existing book
- `DELETE /api/books/{id}` - Delete book
- `GET /api/books/search?searchTerm={term}` - Search books

### Bookmarks

- `GET /api/bookmarks` - Get all bookmarks
- `GET /api/bookmarks/{id}` - Get bookmark by ID
- `POST /api/bookmarks` - Create new bookmark with tags
- `PUT /api/bookmarks/{id}` - Update existing bookmark
- `DELETE /api/bookmarks/{id}` - Delete bookmark
- `GET /api/bookmarks/search?searchTerm={term}` - Search bookmarks
- `GET /api/bookmarks/tags` - Get all available tags

### Health Check

- `GET /health` - Application health status

## Configuration

### Application Settings

The application uses the following configuration structure:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ReaderBuddy.db"
  },
  "ApplicationSettings": {
    "Name": "ReaderBuddy",
    "Version": "1.0.0",
    "EnableDetailedErrors": false
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### Environment-Specific Configuration

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development overrides
- `appsettings.Production.json` - Production overrides (create as needed)

## Database

### Entity Framework Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName --project src/server

# Remove last migration
dotnet ef migrations remove --project src/server

# Note: Database updates happen automatically on application startup
```

### Database Schema

The application includes the following entities:

- **Book**: Represents a book with title, author, ISBN, genre, etc.
- **Bookmark**: User bookmarks with URLs, descriptions, and tags
- **Reading**: Represents a user's reading session for a book
- **Tag**: Categories for organizing bookmarks
- **BookmarkTag**: Many-to-many relationship between bookmarks and tags

### Database Auto-Initialization

The application automatically:
- Creates the SQLite database file on first run
- Applies all pending migrations
- Uses SQLite for development (cross-platform)
- Supports SQL Server for production environments

## Development

### Adding New Features

1. **Models**: Add domain entities in `src/server/Models/`
2. **Services**: Add business logic in `src/server/Services/`
3. **Controllers**: Add API endpoints in `src/server/Controllers/`
4. **React Components**: Add UI components in `src/client/src/components/`
5. **Tests**: Add corresponding tests in `src/tests/`

### Code Quality

- Follow SOLID principles
- Use dependency injection
- Write comprehensive tests
- Follow REST API conventions
- Use proper error handling and logging

## Git Flow and CI/CD

### Branching Strategy

This repository follows the Git Flow branching model:

- **`master`**: Production-ready code, protected branch
- **`develop`**: Integration branch for features
- **`feature/*`**: Feature branches (e.g., `feature/user-authentication`)
- **`release/*`**: Release preparation branches (e.g., `release/1.2.0`)
- **`hotfix/*`**: Critical production fixes (e.g., `hotfix/security-patch`)

### Semantic Versioning

The project uses [GitVersion](https://gitversion.net/) for automatic semantic versioning:

- **Major** (`+semver: major`): Breaking changes
- **Minor** (`+semver: minor`): New features, backwards compatible
- **Patch** (`+semver: patch`): Bug fixes, backwards compatible
- **None** (`+semver: none`): No version increment (documentation, etc.)

Version format: `Major.Minor.Patch[-PreRelease+BuildMetadata]`

### CI/CD Pipeline

The GitHub Actions pipeline (`.github/workflows/ci-cd.yml`) automatically:

1. **Calculates semantic version** using GitVersion
2. **Builds and tests** the .NET solution
3. **Builds React client** with Node.js
4. **Creates Docker images** for master and hotfix branches
5. **Creates Git tags** on master branch releases
6. **Uploads artifacts** for deployment

#### Workflow Triggers

- **Automatic**: Push to any branch, PRs to master/develop
- **Manual**: Workflow dispatch with optional Docker build override

#### Branch-Specific Behaviors

- **`master`**: Full pipeline + Docker build + Git tag creation
- **`develop`**: Build, test, and client build
- **`feature/*`**: Build and test validation
- **`hotfix/*`**: Full pipeline + Docker build
- **`release/*`**: Build, test, and prepare for release

### Local Development Workflow

1. **Clone and setup**:
   ```bash
   git clone <repository-url>
   cd ReaderBuddy
   git checkout develop
   ```

2. **Create feature branch**:
   ```bash
   git checkout -b feature/my-new-feature
   ```

3. **Development cycle**:
   ```bash
   # Make changes
   dotnet build
   dotnet test
   
   # Commit with semantic versioning
   git commit -m "feat: add user authentication +semver: minor"
   ```

4. **Create pull request**:
   - Target: `develop` branch
   - CI/CD pipeline validates changes
   - Code review and merge

### Docker Image Management

Docker images are automatically built for:
- Pushes to `master` branch
- Pushes to `hotfix/*` branches  
- Manual workflow triggers (with override option)

Images are tagged with:
- `latest` (master branch only)
- Semantic version (e.g., `1.2.3`)
- Branch-specific tags for hotfixes

## Deployment

### Docker Deployment

The application includes multi-stage Dockerfiles optimized for production:

1. **Build stage**: Compiles the application
2. **Publish stage**: Creates production artifacts
3. **Runtime stage**: Minimal runtime image

### Environment Variables

Key environment variables for deployment:

- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ConnectionStrings__DefaultConnection`: Database connection string
- `ApplicationSettings__EnableDetailedErrors`: Error detail level

## Contributing

1. Fork the repository
2. Create a feature branch
3. Add tests for new functionality
4. Ensure all tests pass
5. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.