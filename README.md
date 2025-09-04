# .NET 9 REST API with Dapper, PostgreSQL, Clean Architecture

A modern .NET 9 REST API implementing CRUD operations for a blog system with Users, Posts, Comments, and Categories using Dapper as the micro-ORM, PostgreSQL as the database, and following Clean Architecture principles.

## Table of Contents
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Running with Docker](#running-with-docker)
  - [Running Locally](#running-locally)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)
- [Database Schema](#database-schema)
- [Enhancements Implemented](#enhancements-implemented)

## Features

- Full CRUD operations for Users, Posts, Comments, and Categories
- Clean Architecture with separation of concerns
- RESTful API design
- Asynchronous programming throughout
- Docker containerization support
- Swagger/OpenAPI documentation
- Comprehensive unit and integration testing
- PostgreSQL database with Dapper micro-ORM
- Structured logging with different log levels
- Global exception handling middleware
- Input validation and sanitization
- JWT-based authentication and custom authorization
- In-memory caching for improved performance
- CI/CD pipeline with automated deployment and security scanning

## Technology Stack

- **Framework**: .NET 9
- **Language**: C#
- **Database**: PostgreSQL
- **ORM**: Dapper (Micro-ORM)
- **Containerization**: Docker, Docker Compose
- **API Documentation**: Swagger/OpenAPI
- **Testing**: xUnit, Moq
- **Caching**: MemoryCache
- **Logging**: Microsoft.Extensions.Logging
- **Authentication**: JWT Bearer Tokens
- **CI/CD**: GitHub Actions

## Architecture

This project follows Clean Architecture principles with four distinct layers:

```
┌─────────────────────────────────────────────────────────────┐
│                        API LAYER                            │
│  ┌───────────────────────────────────────────────────────┐  │
│  │                 Web API Controllers                   │  │
│  │  - REST endpoints                                     │  │
│  │  - Request/Response mapping                           │  │
│  │  - HTTP status codes                                  │  │
│  └───────────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────────┤
│                     APPLICATION LAYER                       │
│  ┌───────────────────────────────────────────────────────┐  │
│  │                    Use Cases                          │  │
│  │  - Business logic orchestration                       │  │
│  │  - Request validation                                 │  │
│  │  - DTO mapping                                        │  │
│  └───────────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────────┤
│                      DOMAIN LAYER                           │
│  ┌───────────────────────────────────────────────────────┐  │
│  │                    Entities                           │  │
│  │  - Core business objects                              │  │
│  │  - Business rules and validation                      │  │
│  └───────────────────────────────────────────────────────┘  │
├─────────────────────────────────────────────────────────────┤
│                    INFRASTRUCTURE LAYER                     │
│  ┌───────────────────────────────────────────────────────┐  │
│  │                  Data Access                          │  │
│  │  - Dapper repositories                                │  │
│  │  - Database connections                               │  │
│  │  - SQL queries                                        │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

## Project Structure

```
src/
├── WebApi/                 # Presentation Layer (ASP.NET Core Web API)
│   ├── Controllers/        # API Controllers
│   ├── Extensions/         # Service collection extensions
│   ├── Middleware/         # Custom middleware (Exception handling, Authorization)
│   ├── Properties/         # Launch settings
│   ├── appsettings.json    # Configuration
│   └── Program.cs          # Application entry point
├── Application/            # Application Layer
│   ├── DTOs/               # Data Transfer Objects
│   ├── Extensions/         # Service collection extensions
│   ├── Interfaces/         # Service interfaces
│   ├── UseCases/           # Service implementations
│   └── Utilities/          # Utility classes (Input sanitization)
├── Domain/                 # Domain Layer
│   ├── Entities/           # Business entities
│   ├── Exceptions/         # Custom exceptions
│   └── Interfaces/         # Repository interfaces
├── Infrastructure/         # Infrastructure Layer
│   ├── Data/               # Database initialization
│   ├── Extensions/         # Service collection extensions
│   ├── Repositories/       # Dapper repository implementations
│   └── Services/           # Infrastructure services (Caching)
tests/
├── Application.Tests/      # Application layer tests
├── Infrastructure.Tests/   # Infrastructure layer tests
└── WebApi.Tests/           # Web API layer tests
```

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/products/docker-desktop) (for containerization)
- [PostgreSQL](https://www.postgresql.org/download/) (if running locally without Docker)

## Getting Started

### Running with Docker

The easiest way to run the application is with Docker Compose, which will start both the API and PostgreSQL database:

```bash
docker-compose up --build
```

The API will be available at: http://localhost:8081
Swagger documentation: http://localhost:8081/swagger

To stop the services:

```bash
docker-compose down
```

### Running Locally

1. **Set up the database**:
   Make sure PostgreSQL is running and create a database named `blogdb`.

2. **Update connection string** (if needed):
   Modify the connection string in `src/WebApi/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=blogdb;Username=postgres;Password=your_password"
     }
   }
   ```

3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

4. **Build the project**:
   ```bash
   dotnet build
   ```

5. **Run the application**:
   ```bash
   dotnet run --project src/WebApi
   ```

   The API will be available at: https://localhost:5001 or http://localhost:5000
   Swagger documentation: https://localhost:5001/swagger

## API Endpoints

### Authentication
| Method | Endpoint      | Description         |
|--------|---------------|---------------------|
| POST   | `/api/auth/login` | User login to get JWT token |

### Users
| Method | Endpoint        | Description           |
|--------|-----------------|-----------------------|
| GET    | `/api/users`    | Retrieve all users    |
| GET    | `/api/users/{id}`| Retrieve a user by ID |
| POST   | `/api/users`    | Create a new user     |
| PUT    | `/api/users/{id}`| Update a user         |
| DELETE | `/api/users/{id}`| Delete a user         |

### Posts
| Method | Endpoint        | Description           |
|--------|-----------------|-----------------------|
| GET    | `/api/posts`    | Retrieve all posts    |
| GET    | `/api/posts/{id}`| Retrieve a post by ID |
| POST   | `/api/posts`    | Create a new post     |
| PUT    | `/api/posts/{id}`| Update a post         |
| DELETE | `/api/posts/{id}`| Delete a post         |

### Categories
| Method | Endpoint             | Description              |
|--------|----------------------|--------------------------|
| GET    | `/api/categories`    | Retrieve all categories  |
| GET    | `/api/categories/{id}`| Retrieve a category by ID|
| POST   | `/api/categories`    | Create a new category    |
| PUT    | `/api/categories/{id}`| Update a category        |
| DELETE | `/api/categories/{id}`| Delete a category        |

### Comments
| Method | Endpoint            | Description             |
|--------|---------------------|-------------------------|
| GET    | `/api/comments`     | Retrieve all comments   |
| GET    | `/api/comments/{id}` | Retrieve a comment by ID|
| POST   | `/api/comments`     | Create a new comment    |
| PUT    | `/api/comments/{id}` | Update a comment        |
| DELETE | `/api/comments/{id}` | Delete a comment        |

## Testing

The project includes comprehensive unit and integration tests:

```bash
# Run all tests
dotnet test

# Run tests with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests for a specific project
dotnet test tests/Application.Tests
dotnet test tests/Infrastructure.Tests
dotnet test tests/WebApi.Tests
```

## Database Schema

The database consists of four main tables:

1. **Users** - Stores user information
2. **Categories** - Stores post categories
3. **Posts** - Stores blog posts
4. **Comments** - Stores comments on posts

## Enhancements Implemented

### 1. Enhanced Error Handling
- Global exception handling middleware
- Custom exception types for better error management

### 2. Improved Documentation
- XML documentation comments for all public methods and classes
- Enhanced code readability and maintainability

### 3. Added Logging
- Structured logging with ILogger throughout the application
- Configured different log levels for development and production environments

### 4. Enhanced Security
- Input validation and sanitization
- JWT-based authentication
- Custom authorization middleware
- Data annotations for validation

### 5. Improved Testing
- Integration tests for all API endpoints
- Increased code coverage with comprehensive unit tests
- Fixed database concurrency issues in tests

### 6. Added Caching
- In-memory caching implementation using MemoryCache
- Caching for frequently accessed data (categories, posts)
- Cache invalidation strategies

### 7. Enhanced CI/CD Pipeline
- GitHub Actions workflow with multiple jobs:
  - Build and test with coverage reporting
  - Security scanning
  - Performance benchmarking
  - Staging and production deployment
  - Failure notifications
- Health checks in the application
- Optimized Dockerfile for production deployment