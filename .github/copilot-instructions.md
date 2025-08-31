# ReaderBuddy
ReaderBuddy is a .NET application designed for reading assistance and management. This repository is currently in its initial state with foundation files prepared for .NET development.

**ALWAYS follow these instructions first. Only fallback to additional search and context gathering if the information in these instructions is incomplete or found to be in error.**

## Working Effectively
- Verify environment setup:
  - `dotnet --version` -- should show .NET 8.0.119 or later
  - `git --version` -- should show Git 2.51.0 or later
- Current repository state: This is a minimal repository with only LICENSE and .gitignore files
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
  - If it's a console application: Run `dotnet run` and verify expected output
  - If it's a web application: Start the application and verify it responds to requests
  - If it's a library: Run unit tests that exercise the public API
  - Always run `dotnet format` before committing to ensure code formatting consistency
- Currently no functional validation scenarios exist due to minimal codebase state

## Common tasks
The following are outputs from frequently run commands in the current environment.

### Repository root structure
```
ls -la
total 28
drwxr-xr-x 4 runner docker 4096 Aug 31 09:44 .
drwxr-xr-x 3 runner docker 4096 Aug 31 09:41 ..
drwxr-xr-x 7 runner docker 4096 Aug 31 09:44 .git
drwxr-xr-x 2 runner docker 4096 Aug 31 09:45 .github
-rw-r--r-- 1 runner docker 7370 Aug 31 09:41 .gitignore
-rw-r--r-- 1 runner docker 1067 Aug 31 09:41 LICENSE
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

## Development Guidelines
- Use .NET 8+ language features and patterns
- Follow standard .NET project structure conventions
- Place source code in `src/` directory when project grows
- Place tests in `tests/` or `test/` directory when adding tests
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
When this repository develops, expect the following structure:
```
ReaderBuddy/
├── src/
│   ├── ReaderBuddy.Core/        # Core business logic
│   ├── ReaderBuddy.Web/         # Web application (if applicable)
│   └── ReaderBuddy.Console/     # Console application (if applicable)
├── tests/
│   ├── ReaderBuddy.Core.Tests/
│   └── ReaderBuddy.Web.Tests/
├── docs/                        # Documentation
├── .github/
│   └── workflows/               # CI/CD pipelines
├── ReaderBuddy.sln             # Solution file
├── .gitignore
├── LICENSE
└── README.md
```

## Important Notes
- This repository is currently minimal - actual development commands will need validation as the codebase grows
- All build and test timing estimates are for typical .NET projects - actual times may vary
- Always verify commands work in your specific project context before relying on these instructions
- The .gitignore is configured for Visual Studio/.NET development patterns
- MIT license allows for open source development and distribution