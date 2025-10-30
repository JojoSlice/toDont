# toDont

> A minimalist ASP.NET Core Web API for managing anti-todo lists - track what you WON'T do instead of what you will.

## Overview

ASP.NET Core 9.0 Web API with Entity Framework Core and SQLite. Features three entities (ToDont, User, Image) with proper relationships and DTOs.

**Status:** Data layer complete. Controllers need implementation.

## Tech Stack

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0 + SQLite
- Swagger/OpenAPI

## Quick Start

**Prerequisites:** [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

```bash
cd Api
dotnet run
```

Access Swagger UI at `https://localhost:<port>/swagger`

Database (`app.db`) is created automatically on first run.

## Project Structure

```
toDont/
├── Api/
│   ├── Controllers/             # API endpoint controllers
│   ├── DTOs/                    # Data Transfer Objects
│   │   └── ToDontDTO.cs
│   ├── Data/                    # Database context and configurations
│   │   └── AppDbContext.cs
│   ├── Models/                  # Domain models (entities)
│   │   ├── ToDont.cs
│   │   ├── User.cs
│   │   └── Image.cs
│   ├── Program.cs               # Application entry point
│   ├── appsettings.json         # Configuration
│   ├── appsettings.Development.json
│   └── Api.csproj               # Project file
└── README.md
```

## Data Models

### Entity Relationships

```
User (1) ──── (N) ToDont (1) ──── (N) Image
```

- **User** - UserName, PasswordHash, ToDonts collection
- **ToDont** - Title, IsActive, CreatedAt, UpdatedAt, Images collection
- **Image** - FileName, ToDontId (FK)

### DTOs (`Api/DTOs/`)

Uses C# `record` types for API contracts:

- `CreateToDontDto` - For creating new ToDonts
- `UpdateToDontDto` - For updating existing ToDonts
- `ToDontResponseDto` - For API responses

### Database (`AppDbContext`)

SQLite database with three DbSets: `ToDont`, `User`, `Image`

**Connection String:** `Data Source=app.db` (configured in Program.cs:6)

## Development

### Adding New Features

1. Create model in `Api/Models/`
2. Add `DbSet<YourModel>` to `AppDbContext`
3. Create DTOs in `Api/DTOs/`
4. Create controller in `Api/Controllers/`
5. Test via Swagger UI

### Migrations

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

---

## CI/CD

### Automated Build on PR
Runs on `main` and `develop` branches (Ubuntu + Windows)
- NuGet package caching
- Release build with warnings as errors
- Tracks changed files/lines
- Posts build stats to PR

### Manual Testing Workflow
- Build only
- Build and restore
- Check dependencies
- Full test (when tests exist)

## Contributing

1. Create feature branch from `main` or `develop`
2. Keep models and DTOs separate
3. Test via Swagger UI
4. Ensure build passes (no warnings)
5. Submit PR

The automated build will run and comment on your PR.
