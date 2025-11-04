# toDont

> A minimalist ASP.NET Core Web API for managing anti-todo lists - track what you WON'T do instead of what you will.

## Overview

Full-stack application with ASP.NET Core 9.0 Web API backend and React frontend. Features JWT authentication, three entities (ToDont, User, Image) with proper relationships, DTOs, and a service layer architecture.

**Status:** Fully functional API with JWT authentication. Frontend in development.

## Tech Stack

### Backend
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0 + SQLite
- JWT Bearer Authentication
- Swagger/OpenAPI
- xUnit for testing

### Frontend
- React 19
- TypeScript 5.9
- Vite 7
- ESLint

## Quick Start

### Backend API

**Prerequisites:** [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

```bash
cd Api
dotnet run
```

Access Swagger UI at `https://localhost:<port>/swagger`

Database (`app.db`) is created automatically on first run.

### Frontend Client

**Prerequisites:** [Node.js](https://nodejs.org/) (LTS version recommended)

```bash
cd Client
npm install
npm run dev
```

Access the client at `http://localhost:5173` (default Vite port)

## Project Structure

```
toDont/
├── Api/                         # Backend API
│   ├── Controllers/             # API endpoint controllers
│   │   ├── ToDontController.cs
│   │   └── UserController.cs
│   ├── DTOs/                    # Data Transfer Objects
│   │   ├── ToDontDTO.cs
│   │   ├── UserDTO.cs
│   │   └── ImageDTO.cs
│   ├── Data/                    # Database context and configurations
│   │   └── AppDbContext.cs
│   ├── Models/                  # Domain models (entities)
│   │   ├── ToDont.cs
│   │   ├── User.cs
│   │   └── Image.cs
│   ├── Services/                # Business logic layer
│   │   ├── IUserService.cs
│   │   ├── UserService.cs
│   │   ├── IToDontService.cs
│   │   ├── ToDontService.cs
│   │   ├── IJwtService.cs
│   │   └── JwtService.cs
│   ├── Program.cs               # Application entry point
│   ├── appsettings.json         # Configuration (includes JWT settings)
│   └── Api.csproj               # Project file
├── Api.test/                    # Unit tests
│   ├── UnitTest1.cs
│   └── Api.test.csproj
├── Client/                      # React frontend
│   ├── src/
│   │   ├── App.tsx
│   │   └── main.tsx
│   ├── package.json
│   └── vite.config.ts
└── README.md
```

## API Endpoints

### Authentication (Public)
- `POST /api/user/register` - Register new user (returns JWT token)
- `POST /api/user/login` - Login user (returns JWT token)

### User (Requires Authentication)
- `GET /api/user/{id}` - Get user by ID
- `GET /api/user/profile/{username}` - Get user profile by username

### ToDont (Requires Authentication)
- `GET /api/todont` - Get all ToDonts for authenticated user
- `GET /api/todont/{id}` - Get specific ToDont by ID
- `POST /api/todont` - Create new ToDont
- `PUT /api/todont/{id}` - Update ToDont
- `DELETE /api/todont/{id}` - Delete ToDont
- `PATCH /api/todont/{id}/toggle` - Toggle ToDont active status

All ToDont endpoints require a valid JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

Explore all endpoints via Swagger UI at `/swagger` when running in development mode.

## Authentication

The API uses JWT (JSON Web Tokens) for authentication:

1. Register or login to receive a JWT token
2. Include the token in the `Authorization` header for protected endpoints
3. Tokens expire after 24 hours (configurable in `appsettings.json`)

**JWT Configuration** (`Api/appsettings.json`):
```json
{
  "Jwt": {
    "Key": "YOUR_SECRET_KEY_AT_LEAST_32_CHARACTERS",
    "Issuer": "ToDontApi",
    "Audience": "ToDontClient",
    "ExpiryInHours": 24
  }
}
```

**Security Note:** Change the JWT Key in production and keep it secure. Never commit production secrets to version control.

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

### Architecture

The project follows a clean architecture pattern with:
- **Controllers** - Handle HTTP requests/responses
- **Services** - Contain business logic
- **DTOs** - Define API contracts
- **Models** - Define database entities
- **Data** - Database context and configuration

### Adding New Features

1. Create model in `Api/Models/`
2. Add `DbSet<YourModel>` to `AppDbContext`
3. Create DTOs in `Api/DTOs/`
4. Create service interface in `Api/Services/I<YourService>.cs`
5. Implement service in `Api/Services/<YourService>.cs`
6. Register service in `Program.cs` with dependency injection
7. Create controller in `Api/Controllers/`
8. Test via Swagger UI

### Running Tests

```bash
cd Api.test
dotnet test
```

### Migrations

```bash
cd Api
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Frontend Development

```bash
cd Client
npm run dev      # Start development server
npm run build    # Build for production
npm run lint     # Run ESLint
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
2. Follow the clean architecture pattern (Models → Services → Controllers)
3. Keep models and DTOs separate
4. Register new services in `Program.cs` for dependency injection
5. Write unit tests in `Api.test/` for new features
6. Test via Swagger UI
7. Ensure build passes (no warnings)
8. Ensure all tests pass (`dotnet test`)
9. Submit PR

The automated build will run and comment on your PR.
