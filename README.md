# CleanCodeScaffold
CleanCodeScaffold project is a Clean Code Architecture template designed for .NET Core 6 and 8. It incorporates essential components such as Identity Framework for user management, a Generic Repository for data access, and Serilog with Seq for robust logging. Ideal for building modular, maintainable, and scalable applications following best practices in software architecture.

**Key Features**:

**Clean Code Architecture**: Follows the Hexagonal (Ports and Adapters) Architecture pattern, separating core business logic from external concerns.

**Generic CRUD Operations**: Provides generic implementations for Create, Read, Update, and Delete operations, allowing you to focus on domain-specific logic.

**Dependency Injection**: Utilizes .NET Core's built-in Dependency Injection to manage object lifetimes and facilitate loosely coupled components.

**Modular Structure**: Organized into layers (Presentation, Application, Domain, and Infrastructure) for clear separation of concerns, making it easier to maintain and extend the application.

**Testing Support**: Designed with testability in mind, allowing you to write unit tests and ensure the reliability of your codebase.

**Getting Started**:

Choose Your Preferred Approach:

**Via NuGet Package:**

Install the template using the NuGet package.  
    dotnet new -i CleanCodeScaffold  
Create a new project with the installed template.  
    dotnet new CleanCodeScaffold -n YourNewProjectName  
Run your project.  

**Via Repository:**

Clone the repository to your local machine.  
    git clone https://github.com/waleed415/CleanCodeScaffold.git  
Navigate to the template project within the repository.  
    cd /CleanCodeScaffold  
Install the template.  
    dotnet new -i .  
Create new project using visual studio chose CleanCodeScaffold.  

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
