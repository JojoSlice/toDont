# toDont

> A minimalist ASP.NET Core Web API for managing anti-todo lists - track what you WON'T do instead of what you will.

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Architecture Guide](#architecture-guide)
  - [Models](#models)
  - [DTOs](#dtos)
  - [Data Layer](#data-layer)
- [Development Guidelines](#development-guidelines)
- [Database](#database)
- [API Documentation](#api-documentation)

## Overview

**toDont** is a clean, lightweight ASP.NET Core 9.0 Web API project built with Entity Framework Core and SQLite. The project follows a straightforward architecture pattern suitable for small to medium-sized applications.

## Tech Stack

- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core 9.0** - ORM for database operations
- **SQLite** - Lightweight embedded database
- **Swagger/OpenAPI** - API documentation and testing interface

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd toDont
   ```

2. **Navigate to the API project**
   ```bash
   cd Api
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access Swagger UI** (Development mode only)
   ```
   https://localhost:<port>/swagger
   ```

The database (`app.db`) will be created automatically on first run.

### Building the Project

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

## Project Structure

```
toDont/
├── Api/
│   ├── Controllers/         # API endpoint controllers
│   ├── Data/               # Database context and configurations
│   │   └── AppDbContext.cs
│   ├── Models/             # Domain models (entities)
│   │   └── ToDont.cs
│   ├── Program.cs          # Application entry point
│   ├── appsettings.json    # Configuration
│   └── Api.csproj          # Project file
└── README.md
```

## Architecture Guide

This project follows a simple layered architecture suitable for straightforward CRUD applications. As the project grows, consider implementing the repository pattern, service layer, and DTOs.

### Models

**Location:** `Api/Models/`

Models represent your **domain entities** - the core business objects that map directly to database tables via Entity Framework Core.

#### Current Model: `ToDont`

```csharp
namespace Models
{
    public class ToDont
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
```

#### Model Guidelines

- **Purpose:** Models define the structure of your database tables
- **Location:** All models should live in the `Models/` directory
- **Naming:** Use singular nouns (e.g., `ToDont`, not `ToDonts`)
- **Properties:**
  - Include an `Id` property for the primary key
  - Use appropriate data types that match your business requirements
  - Consider nullable reference types for .NET 9.0
- **Data Annotations:** Add validation and schema attributes as needed:
  ```csharp
  [Required]
  [MaxLength(200)]
  public string Title { get; set; }
  ```

### DTOs

**Location:** `Api/DTOs/` (to be created as needed)

DTOs (Data Transfer Objects) are lightweight objects used to transfer data between layers, specifically between your API and clients.

#### When to Use DTOs

- **API Responses:** Return DTOs instead of domain models to control what data is exposed
- **API Requests:** Accept DTOs to validate and shape incoming data
- **Projection:** Select only the fields needed for a specific operation
- **Decoupling:** Separate your API contract from your database schema

#### DTO Best Practices

```csharp
// Example: Request DTO for creating a ToDont
public class CreateToDontDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
}

// Example: Response DTO
public class ToDontDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
}
```

**Guidelines:**
- Suffix DTOs with `Dto` (e.g., `CreateToDontDto`, `UpdateToDontDto`)
- Create separate DTOs for Create, Update, and Response scenarios
- Only include properties that should be exposed/accepted
- Add validation attributes (`[Required]`, `[MaxLength]`, etc.)
- **Never expose your domain models directly through API endpoints**

#### Model vs DTO Decision Matrix

| Scenario | Use Model | Use DTO |
|----------|-----------|---------|
| Database operations | ✅ | ❌ |
| Internal business logic | ✅ | ❌ |
| API request bodies | ❌ | ✅ |
| API responses | ❌ | ✅ |
| Entity Framework queries | ✅ | ❌ |
| Validation rules | Both | ✅ (primarily) |

### Data Layer

**Location:** `Api/Data/`

The data layer contains your Entity Framework Core `DbContext` and database configurations.

#### `AppDbContext`

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<ToDont> toDont { get; set; }
}
```

**Configuration** (from `Program.cs:6`):
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));
```

#### Adding New Entities

1. Create the model in `Models/`
2. Add a `DbSet<YourModel>` property to `AppDbContext`
3. Create and apply migrations (if using migrations)
4. Create corresponding DTOs in `DTOs/`

## Development Guidelines

### Adding a New Feature

1. **Create the Model** in `Api/Models/`
2. **Update DbContext** - Add `DbSet<YourModel>` to `AppDbContext.cs`
3. **Create DTOs** in `Api/DTOs/` (if needed)
4. **Create Controller** in `Api/Controllers/`
5. **Test** using Swagger UI in development mode

### Code Style

- Use meaningful, descriptive names
- Follow C# naming conventions (PascalCase for classes, camelCase for parameters)
- Keep controllers thin - move business logic to services as the project grows
- Use async/await for database operations
- Enable nullable reference types

### Entity Framework Core Tips

- The database is created automatically via `db.Database.EnsureCreated()` in `Program.cs:23`
- For production, consider using **migrations** instead:
  ```bash
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```

## Database

**Type:** SQLite
**File:** `app.db` (created in the project root)
**Connection String:** `Data Source=app.db`

The database is automatically created on application startup if it doesn't exist.

### Schema

#### `toDont` Table

| Column | Type | Constraints |
|--------|------|-------------|
| Id | INTEGER | PRIMARY KEY, AUTO INCREMENT |
| Title | TEXT | NOT NULL |
| IsActive | INTEGER | NOT NULL (boolean) |

## API Documentation

When running in development mode, comprehensive API documentation is available via Swagger UI at:

```
https://localhost:<port>/swagger
```

Swagger provides:
- Interactive API testing
- Request/response schemas
- Endpoint documentation
- Try-it-out functionality

---

## Contributing

When contributing to this project:

1. Create a feature branch from `main`
2. Follow the architecture guidelines above
3. Keep models and DTOs separate
4. Write clean, self-documenting code
5. Test your endpoints via Swagger
6. Submit a pull request

## Questions?

For questions or issues, please open an issue in the repository or contact the development team.
