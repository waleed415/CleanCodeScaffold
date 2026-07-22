# CleanCodeScaffold
CleanCodeScaffold project is a Clean Code Architecture template designed for .NET Core 6, 8 and 10. It incorporates essential components such as Identity Framework for user management, a Generic Repository for data access, and Serilog with Seq for robust logging. Ideal for building modular, maintainable, and scalable applications following best practices in software architecture.

## Key Features

- **Production-Ready Identity Module**
   Skip the boilerplate. Includes fully implemented **User Registration, Login, Forgot Password, and Role Management** using ASP.NET Core Identity.

- **Hexagonal Architecture**  
  Uses a Ports-and-Adapters approach to keep business logic independent of frameworks, databases, and external services.

- **Multi-Version .NET Support**  
  Create projects targeting .NET 6, .NET 8, or .NET 10.

- **Generic CRUD and Search**  
  Includes reusable CRUD operations and View Model-based generic search to speed up development of standard business modules.

- **Demo Module Included**  
  Provides a working example that demonstrates how to structure entities, View Models, validation, mappings, services, repositories, and API endpoints.

- **AutoMapper and FluentValidation**  
  Uses AutoMapper for predictable object mapping and FluentValidation for clean, reusable request validation.

- **Structured Logging and Observability**  
  Uses Serilog and Seq to capture structured application logs for easier troubleshooting and monitoring.

- **PDF Reporting Examples**  
  Includes examples for generating PDF reports that can be adapted for invoices, summaries, exports, and operational reports.

- **No MediatR Dependency**  
  Keeps application flow explicit and easy to follow through direct application services, while retaining separation of concerns and testability.

- **Extensible Layered Design**  
  Organizes code into Presentation, Application, Domain, and Infrastructure layers to support long-term maintainability.
**Getting Started**:

Choose Your Preferred Approach:

**Via NuGet Package:**

Install the template using the NuGet package.  
    dotnet new -i CleanCodeScaffold  
Create a new project with the installed template.  
    dotnet new CCScaffold -n [ProjectName] --framework [net10.0/net8.0/net6.0] --connectionString "[your-connection-string]" --secretKey "[replace-this-with-a-secure-long-random-jwt-secret]"  

if you are chosing framework net10.0 then execute add migration command
Run your project.  

**Via Repository:**

Clone the repository to your local machine.  
    git clone https://github.com/waleed415/CleanCodeScaffold.git  
Navigate to the template project within the repository.  
    cd /CleanCodeScaffold  
Install the template.  
    dotnet new -i .  
Create new project using visual studio chose CleanCodeScaffold.  

**Via Command**

if you are using the VSCode then you can use the following command for creating the project 

dotnet new CCScaffold -n [ProjectName] --framework [net10.0/net8.0/net6.0] --connectionString "[your-connection-string]" --secretKey "[replace-this-with-a-secure-long-random-jwt-secret]"

**Contributing**:

Contributions are welcome! Feel free to fork this repository, open issues, and submit pull requests to help improve the CleanCodeScaffold project.

CleanCodeScaffold  
│  
├───src  
│   │  
│   ├───CleanCodeScaffold.Application         (Application Layer)  
│   │   ├───Commands                         (Use Case Commands)  
│   │   ├───Queries                          (Use Case Queries)  
│   │   ├───Services                         (Application Services)  
│   │   ├───Mappers                          (Data Mappers)  
│   │   ├───Responses                         (Response Models)  
│   │   └───Authenticators                   (Authentication Logic)  
│   │  
│   ├───CleanCodeScaffold.Domain             (Domain Layer)  
│   │   ├───Entities                         (Domain Entities)  
│   │   ├───ValueObjects                     (Domain Value Objects)  
│   │   └───Interfaces                        (Domain Interfaces)  
│   │  
│   ├───CleanCodeScaffold.Infrastructure     (Infrastructure Layer)  
│   │   ├───Persistence                      (Database Access, Repositories)  
│   │   ├───ExternalServices                  (External APIs, Third-party Services)  
│   │   └───Messaging                         (Message Brokers, Email Services)  
│   │  
│   └───CleanCodeScaffold.Api                (Presentation Layer - API)  
│       ├───Controllers                      (API Endpoints)  
│       ├───Util                              (Utility classes, helpers, etc.)  
│       └───Program.cs                        (API Entry Point)  
│  
└───tools  
    └───build   
