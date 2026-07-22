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

```bash
dotnet new CCScaffold -n YourProjectName --framework net8.0 --connectionString "your-connection-string" --secretKey "replace-this-with-a-secure-long-random-jwt-secret"
```

Example:

```bash
dotnet new CCScaffold -n BillingManagementApi --framework net8.0 --connectionString "Server=localhost;Database=BillingManagementDb;Trusted_Connection=True;TrustServerCertificate=True;" --secretKey "replace-this-with-a-secure-long-random-jwt-secret"
```

For .NET 10 projects, run the migration command after creating the project.

To see all available options:

```bash
dotnet new CCScaffold --help
```

## Install from Repository

Clone the repository:

```bash
git clone https://github.com/waleed415/CleanCodeScaffold.git
```

Navigate to the project directory:

```bash
cd CleanCodeScaffold
```

Install the template locally:

```bash
dotnet new -i .
```

Create a new project:

```bash
dotnet new CCScaffold -n YourProjectName --framework net8.0 --connectionString "your-connection-string" --secretKey "replace-this-with-a-secure-long-random-jwt-secret"
```

## Running the Application

After creating your project, update the required configuration values, including:

- Database connection string
- JWT secret key
- Identity configuration
- Logging and Seq configuration, if applicable

Restore and run the project:

```bash
dotnet restore
dotnet run
```

## Authentication and Identity Features

CleanCodeScaffold includes a ready-to-use foundation for common account-management requirements:

- User registration
- User login
- Forgot-password flow
- User management
- Role management
- ASP.NET Core Identity integration

This helps teams begin business development without spending initial project time rebuilding standard authentication functionality.

## Generic CRUD and Search

The template provides reusable patterns for common business operations:

- Create records
- Retrieve records by ID
- Update records
- Delete records
- List records
- Search and filter records
- ViewModel-based response models
- Generic CRUD implementation
- Demo CRUD module for reference

When creating a new business module, the recommended process is:

1. Create a domain entity.
2. Create request and response View Models.
3. Configure AutoMapper mappings.
4. Add FluentValidation rules.
5. Implement application service logic.
6. Add repository and persistence logic.
7. Expose the module through API endpoints.

## PDF Reporting

CleanCodeScaffold includes PDF-report-generation examples that can be adapted for:

- Invoices
- Customer statements
- Sales summaries
- Operational reports
- Exportable business documents
- Printable management reports

## Logging and Observability

Structured logging is implemented using **Serilog**.

The template also supports **Seq** for centralized logging and analysis, helping teams investigate:

- API requests and responses
- Application exceptions
- Authentication events
- Integration failures
- Business-operation failures
- Performance issues

## Why No MediatR?

CleanCodeScaffold does not require MediatR by default.

The template uses direct application services to keep request flow explicit and easy to follow:

```text
Controller → Application Service → Domain / Repository → Response
```

This approach is useful for teams building business applications, enterprise APIs, and integration services where readability, debugging, and onboarding speed are priorities.

MediatR can still be introduced later when a project has complex CQRS or event-driven requirements that benefit from a mediator pattern.

## Recommended Use Cases

CleanCodeScaffold is suitable for:

- Enterprise Web APIs
- ERP modules
- Billing and invoicing systems
- Finance and accounting applications
- Internal business portals
- CRM systems
- Healthcare administration systems
- Government and third-party integration services
- SaaS back-office applications
- Reporting and dashboard APIs
- Inventory and order-management systems

## Technology Stack

- C#
- ASP.NET Core
- .NET 6, .NET 8, and .NET 10
- Entity Framework Core
- ASP.NET Core Identity
- AutoMapper
- FluentValidation
- Serilog
- Seq
- Generic Repository Pattern
- REST APIs
- Clean Architecture
- Hexagonal Architecture / Ports and Adapters

## Roadmap

Potential future improvements include:

- Additional unit and integration test examples
- Docker and Docker Compose setup
- CI/CD pipeline examples
- More demo business modules
- Additional database-provider options
- Caching and messaging examples
- Enhanced API documentation
- Expanded reporting examples
- More authorization and security scenarios

## Contributing

Contributions, bug reports, feedback, and feature requests are welcome.

To contribute:

1. Fork the repository.
2. Create a feature branch.
3. Make your changes.
4. Update relevant tests and documentation.
5. Submit a pull request.

For significant changes, please open an issue first to discuss the proposed approach.

## Links

- **GitHub Repository:** [CleanCodeScaffold](https://github.com/waleed415/CleanCodeScaffold)
- **NuGet Package:** [CleanCodeScaffold](https://www.nuget.org/packages/CleanCodeScaffold)

## License

This project is available under the repository’s existing license.
