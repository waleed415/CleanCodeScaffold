# CleanCodeScaffold

A practical, production-oriented **.NET Clean Architecture template** for building secure, maintainable, and scalable business applications and APIs.

CleanCodeScaffold follows **Hexagonal Architecture (Ports and Adapters)** and provides commonly required application features from the start—including authentication flows, generic CRUD, ViewModel-based search, validation, structured logging, and PDF report examples.

It is designed for teams building enterprise APIs, ERP modules, billing systems, internal portals, integration platforms, and back-office applications without starting from an empty project structure.

## Supported Frameworks

- .NET 6
- .NET 8
- .NET 10

## Key Features

- **Hexagonal / Clean Architecture**
  - Follows the Ports and Adapters pattern.
  - Separates core business logic from databases, frameworks, UI, and external services.
  - Uses a modular structure with Presentation, Application, Domain, and Infrastructure layers.

- **Ready-to-Use Identity Module**
  - User registration
  - User login
  - Forgot-password flow
  - User and role-management foundation
  - Built using ASP.NET Core Identity

- **Generic CRUD Operations**
  - Reusable Create, Read, Update, and Delete implementation.
  - Helps teams build standard business modules using consistent patterns.

- **ViewModel-Based Generic Search**
  - Reusable search and listing functionality based on View Models.
  - Supports common filtering, paging, and data-listing requirements.

- **Demo CRUD Module**
  - Includes working CRUD examples to demonstrate the recommended implementation approach.

- **PDF Report Generation Examples**
  - Includes examples for generating PDF reports.
  - Can be adapted for invoices, summaries, exports, and operational reports.

- **AutoMapper**
  - Simplifies mapping between entities, DTOs, request models, and response models.

- **FluentValidation**
  - Keeps request validation clean, reusable, and separate from controller logic.

- **Generic Repository Pattern**
  - Provides a standardized data-access approach.
  - Helps separate persistence concerns from application and domain logic.

- **Structured Logging and Observability**
  - Uses Serilog for structured logging.
  - Supports Seq for centralized log analysis, troubleshooting, and observability.

- **Dependency Injection**
  - Uses built-in .NET dependency injection for loosely coupled and testable components.

- **No Mandatory MediatR Dependency**
  - Uses explicit application services and direct request flow.
  - Keeps the codebase easier to understand, debug, and maintain.
  - Teams can add MediatR later if their application complexity requires it.

- **Modular and Testable Design**
  - Designed with separation of concerns in mind.
  - Supports unit and integration testing practices.

## Architecture Overview

CleanCodeScaffold follows Clean Architecture and Hexagonal Architecture principles.

```text
CleanCodeScaffold
│
├── src
│   │
│   ├── CleanCodeScaffold.Application
│   │   ├── Commands
│   │   ├── Queries
│   │   ├── Services
│   │   ├── Mappers
│   │   ├── Responses
│   │   └── Authenticators
│   │
│   ├── CleanCodeScaffold.Domain
│   │   ├── Entities
│   │   ├── ValueObjects
│   │   └── Interfaces
│   │
│   ├── CleanCodeScaffold.Infrastructure
│   │   ├── Persistence
│   │   ├── ExternalServices
│   │   └── Messaging
│   │
│   └── CleanCodeScaffold.Api
│       ├── Controllers
│       ├── Util
│       └── Program.cs
│
└── tools
    └── build
```

### Layer Responsibilities

| Layer | Responsibility |
|---|---|
| `CleanCodeScaffold.Api` | API endpoints, controllers, middleware, dependency registration, and HTTP concerns |
| `CleanCodeScaffold.Application` | Business use cases, application services, commands, queries, mappings, validation, and response models |
| `CleanCodeScaffold.Domain` | Core business entities, value objects, domain rules, and interfaces |
| `CleanCodeScaffold.Infrastructure` | Persistence, repositories, Identity, external APIs, messaging, reporting, and framework-specific implementations |

## Getting Started

### Prerequisites

Before starting, install:

- .NET SDK 6, 8, or 10
- A supported database server and connection string
- Seq, if you want to use centralized structured logging

## Install via NuGet

Install the template:

```bash
dotnet new -i CleanCodeScaffold
```

Create a new project:

### PostgreSQL Example:
```bash
dotnet new CCScaffold -n BillingManagementApi --framework net8.0 --connectionString "Server=localhost;Database=BillingManagementDb;User Id=postgres;Password=password;" --secretKey "secure-jwt-secret" --databaseProvider "PostgreSQL"
```

### MS SQL Server Example:
```bash
dotnet new CCScaffold -n BillingManagementApi --framework net8.0 --connectionString "Server=localhost;Database=BillingManagementDb;Trusted_Connection=True;" --secretKey "secure-jwt-secret" --databaseProvider "MS SQL Server"
```

To see all available options:

```bash
dotnet new CCScaffold --help
```

## Running the Application

1. Apply migrations:
```bash
dotnet ef migrations add InitialCreate --project src/YOUR_INFRASTRUCTURE_PROJECT --startup-project src/YOUR_API_PROJECT
```

2. Restore and run:
```bash
dotnet restore
dotnet run --project src/YOUR_API_PROJECT
```

## Authentication and Identity Features

CleanCodeScaffold includes a ready-to-use foundation for common account-management requirements:

- User registration, login, and forgot-password flow.
- User management and Role-based authorization.
- ASP.NET Core Identity & JWT integration.

## Generic CRUD and Search

The template provides reusable patterns for:

- Full CRUD implementation.
- ViewModel-based generic search and filtering.
- Demo CRUD module for reference.

## PDF Reporting

CleanCodeScaffold includes PDF-report-generation examples for:

- Invoices, statements, and operational summaries.

## Technology Stack

- C#, .NET 6, 8, & 10
- Entity Framework Core (SQL Server / PostgreSQL)
- ASP.NET Core Identity
- AutoMapper & FluentValidation
- Serilog & Seq
- Hexagonal Architecture

## Links

- **GitHub Repository:** [CleanCodeScaffold](https://github.com/waleed415/CleanCodeScaffold)
- **NuGet Package:** [CleanCodeScaffold](https://www.nuget.org/packages/CleanCodeScaffold)

## License

This project is available under the repository’s existing license.
