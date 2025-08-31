# ReaderBuddy
ReaderBuddy is a .NET application designed for reading assistance and management. This repository includes a comprehensive CI/CD pipeline with Git Flow, semantic versioning, and automated Docker builds.

**ALWAYS follow these instructions first. Only fallback to additional search and context gathering if the information in these instructions is incomplete or found to be in error.**

## Git Flow and Branching Strategy
- **master**: Production-ready code, receives merges from release branches and hotfixes
- **develop**: Integration branch for features, used for ongoing development  
- **feature/***: Feature branches created from develop, merged back to develop
- **release/***: Release preparation branches created from develop, merged to master and develop
- **hotfix/***: Critical fixes created from master, merged to master and develop
- GitVersion automatically calculates semantic versions based on branch patterns
- Tags are automatically created on master branch pushes using format `v{version}`

## CI/CD Pipeline
- **Automated builds**: Triggered on push to master, develop, release/*, hotfix/*, feature/*
- **Pull request validation**: Builds and tests run on PRs to master/develop
- **Semantic versioning**: Uses GitVersion with configuration in `GitVersion.yml`
- **Docker builds**: Automatic on master/hotfix branches, manual option available
- **Artifacts**: Publishes .NET API, React client, and Docker images
- **Manual triggers**: Workflow can be manually triggered with option to force Docker build

## Working Effectively
- Verify environment setup:
  - `dotnet --version` -- should show .NET 8.0.119 or later
  - `git --version` -- should show Git 2.51.0 or later
- Current repository state: Full-featured .NET 8 WebAPI with React client, CI/CD pipeline, and Docker support
- Project initialization (when creating new projects):
  - `dotnet new list` -- view available .NET project templates
  - `dotnet new sln -n ReaderBuddy` -- create solution file
  - `dotnet new console -n ReaderBuddy.Console` -- example console app creation
  - `dotnet new classlib -n ReaderBuddy.Core` -- example class library creation
  - `dotnet sln add **/*.csproj` -- add all projects to solution
- Build and test (when project files exist):
  - `dotnet restore` -- restore NuGet packages (typically takes 1-2 seconds for small projects)
  - `dotnet build` -- build the solution (typically takes 5-10 seconds for small projects). NEVER CANCEL. Set timeout to 120+ seconds.
  - `dotnet test` -- run unit tests (typically takes 5-10 seconds for basic tests). NEVER CANCEL. Set timeout to 120+ seconds.
  - **CRITICAL**: NEVER CANCEL build or test operations - always wait for completion

## Validation
- Always validate that your environment has the required .NET SDK before starting development
- When adding new projects, always run `dotnet build` to ensure compilation succeeds
- If unit tests exist, always run `dotnet test` after making changes
- Before committing, verify the solution builds successfully
- **VALIDATION SCENARIOS**: When this codebase has actual functionality, always test:
  - Web API: Run `dotnet run --project src/ReaderBuddy.WebApi` and verify API responds at https://localhost:7274
  - React client: Run `npm start` in client directory and verify UI loads at http://localhost:3000  
  - Docker: Build with `docker build -t readerbuddy .` and run with `docker run -p 8080:8080 readerbuddy`
  - Integration: Verify React client can communicate with API backend
  - Always run `dotnet format` before committing to ensure code formatting consistency
- Currently no functional validation scenarios exist due to minimal codebase state

## Common tasks
The following are outputs from frequently run commands in the current environment.

### Repository root structure
```
ls -la
total 64
drwxr-xr-x 7 runner docker 4096 Aug 31 10:36 .
drwxr-xr-x 3 runner docker 4096 Aug 31 10:35 ..
drwxr-xr-x 7 runner docker 4096 Aug 31 10:36 .git
drwxr-xr-x 2 runner docker 4096 Aug 31 10:36 .github
-rw-r--r-- 1 runner docker 7631 Aug 31 10:36 .gitignore
-rw-r--r-- 1 runner docker  740 Aug 31 10:36 Dockerfile
-rw-r--r-- 1 runner docker 1067 Aug 31 10:36 LICENSE
-rw-r--r-- 1 runner docker 7853 Aug 31 10:36 README.md
-rw-r--r-- 1 runner docker 2041 Aug 31 10:36 ReaderBuddy.sln
drwxr-xr-x 4 runner docker 4096 Aug 31 10:36 client
-rw-r--r-- 1 runner docker 1202 Aug 31 10:36 docker-compose.dev.yml
-rw-r--r-- 1 runner docker 1144 Aug 31 10:36 docker-compose.yml
-rw-r--r-- 1 runner docker 1466 Aug 31 10:36 GitVersion.yml
drwxr-xr-x 3 runner docker 4096 Aug 31 10:36 src
drwxr-xr-x 3 runner docker 4096 Aug 31 10:36 tests
```

### Environment verification
```
dotnet --version
8.0.119

dotnet --list-sdks
8.0.119 [/usr/lib/dotnet/sdk]

git --version
git version 2.51.0
```

### Available .NET templates
```
dotnet new list
Template Name                                 Short Name                  Language    Tags                      
--------------------------------------------  --------------------------  ----------  --------------------------
ASP.NET Core Web API                          webapi                      [C#],F#     Web/Web API/API/Service   
ASP.NET Core Web App (Model-View-Controller)  mvc                         [C#],F#     Web/MVC                   
ASP.NET Core Web App (Razor Pages)            webapp,razor                [C#]        Web/MVC/Razor Pages       
Blazor Web App                                blazor                      [C#]        Web/Blazor/WebAssembly    
Class Library                                 classlib                    [C#],F#,VB  Common/Library            
Console App                                   console                     [C#],F#,VB  Common/Console            
Solution File                                 sln,solution                            Solution                  
xUnit Test Project                            xunit                       [C#],F#,VB  Test/xUnit
```

## CI/CD Commands
When working with the CI/CD pipeline:
- `git push origin master` -- triggers full CI/CD pipeline with Docker build and tagging  
- `git push origin develop` -- triggers build and test pipeline
- `git push origin feature/branch-name` -- triggers build and test validation
- GitHub Actions can be manually triggered from the Actions tab with optional Docker build
- GitVersion automatically calculates versions: `dotnet tool install --global GitVersion.Tool && dotnet gitversion`
- Local Docker build: `docker build -t readerbuddy .` 

## Development Guidelines
- Follow Git Flow branching model (master, develop, feature/*, release/*, hotfix/*)
- Use semantic commit messages for automatic version bumping (+semver: major/minor/patch/none)
- CI/CD pipeline runs on all branch pushes and pull requests
- Docker images built automatically on master and hotfix branches
- All builds must pass before merging to master or develop
- Use .NET 8+ language features and patterns
- Follow standard .NET project structure conventions
- Use xUnit for unit testing (most common in .NET ecosystem)
- Consider using ASP.NET Core for web applications given the "ReaderBuddy" name suggests a user-facing application
- **TIMING EXPECTATIONS**:
  - Small projects: Build takes 5-10 seconds, restore takes 1-2 seconds, tests take 5-10 seconds
  - Medium projects: Build takes 30-60 seconds, restore takes 10-30 seconds, tests take 30-60 seconds  
  - Large projects: Build takes 2-5 minutes, restore takes 30-60 seconds, tests take 2-10 minutes
  - **NEVER CANCEL**: Always set timeouts to at least double the expected time
- When the project grows, consider using:
  - `dotnet format` for code formatting (takes 1-5 seconds)
  - `dotnet pack` for creating NuGet packages if creating libraries
  - `dotnet publish` for deployment scenarios (takes 10-30 seconds for small apps)

## Project Structure Expectations
Current repository structure:
```
ReaderBuddy/
├── src/
│   └── ReaderBuddy.WebApi/          # ASP.NET Core Web API
├── tests/
│   └── ReaderBuddy.WebApi.Tests/    # xUnit test project
├── client/                          # React TypeScript UI
├── docs/                           # Documentation (if needed)
├── .github/
│   ├── workflows/                  # CI/CD GitHub Actions
│   └── copilot-instructions.md     # This file
├── Dockerfile                      # Multi-stage Docker build
├── docker-compose.yml             # Production Docker Compose
├── docker-compose.dev.yml         # Development Docker Compose
├── GitVersion.yml                 # Semantic versioning configuration
├── ReaderBuddy.sln               # .NET solution file
├── .gitignore
├── LICENSE
└── README.md
```

## Important Notes
- Repository has complete CI/CD pipeline with semantic versioning and Docker support
- All build and test timing estimates are for typical .NET projects - actual times may vary
- Always verify commands work in your specific project context before relying on these instructions
- The .gitignore is configured for Visual Studio/.NET development patterns
- MIT license allows for open source development and distribution
- GitVersion configuration supports Git Flow branching model
- CI/CD pipeline automatically builds Docker images for deployment readiness
- Manual workflow triggers available for testing CI/CD changes