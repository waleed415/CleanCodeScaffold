# CleanCodeScaffold
CleanCodeScaffold is a .NET Core project template following Clean Code Architecture. It provides generic CRUD operations, modular layers, and easy testing. Start your projects with clean, maintainable code. Contributions welcome!

CleanCodeScaffold is a generic CRUD (Create, Read, Update, Delete) project template built on the principles of Clean Code Architecture. It serves as a foundation for developing robust, maintainable, and scalable .NET Core applications while emphasizing clean, modular, and testable code practices.

**Key Features**:

**Clean Code Architecture**: Follows the Hexagonal (Ports and Adapters) Architecture pattern, separating core business logic from external concerns.

**Generic CRUD Operations**: Provides generic implementations for Create, Read, Update, and Delete operations, allowing you to focus on domain-specific logic.

**Dependency Injection**: Utilizes .NET Core's built-in Dependency Injection to manage object lifetimes and facilitate loosely coupled components.

**Modular Structure**: Organized into layers (Presentation, Application, Domain, and Infrastructure) for clear separation of concerns, making it easier to maintain and extend the application.

**Testing Support**: Designed with testability in mind, allowing you to write unit tests and ensure the reliability of your codebase.

**Getting Started**:

Clone this repository.
Install the required dependencies using .NET CLI or your preferred package manager.
Customize the domain-specific logic in the Application Layer.
Run the application and start building your features on top of the clean architecture foundation.

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
