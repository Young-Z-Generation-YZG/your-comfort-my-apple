# Project Structure

## Root Organization

```
YGZ.CA.Microservices/
├── apps/                    # Frontend applications
├── Services/                # Backend microservices
├── Gateways/               # API Gateway (YARP)
├── blockchain/             # Solana payment programs
├── docs/                   # Documentation
├── provision/              # DevOps and deployment configs
├── containers/             # Docker volume data
└── docker-compose.yml      # Container orchestration
```

## Frontend Structure (`apps/`)

Both client and admin apps follow Next.js 14/15 conventions:

```
apps/client/ (or admin/)
├── src/
│   ├── app/                # Next.js App Router pages
│   ├── components/         # React components
│   ├── domain/            # Business logic and types
│   ├── infrastructure/    # API clients, services
│   ├── hooks/             # Custom React hooks
│   └── middlewares/       # Next.js middleware
├── public/                # Static assets
├── assets/                # Fonts, images, SVGs
├── globals.css            # Global styles
├── tailwind.config.ts     # Tailwind configuration
└── package.json
```

### Frontend Conventions

-   Use `src/app/` for pages (App Router)
-   Place reusable components in `src/components/`
-   Domain logic goes in `src/domain/`
-   API calls in `src/infrastructure/`
-   Use TypeScript for all files
-   Follow kebab-case for file names
-   Use PascalCase for component files

## Backend Structure (`Services/`)

Each microservice follows Clean Architecture with 4 layers:

```
Services/ServiceName/
├── YGZ.ServiceName.Api/              # Presentation Layer
│   ├── Controllers/                  # API endpoints
│   ├── Contracts/                    # Request DTOs
│   ├── Extensions/                   # Service registrations
│   ├── Middlewares/                  # Custom middleware
│   ├── Program.cs                    # Application entry point
│   └── appsettings.json
│
├── YGZ.ServiceName.Application/      # Application Layer
│   ├── Features/
│   │   ├── Commands/                 # Write operations (CQRS)
│   │   └── Queries/                  # Read operations (CQRS)
│   ├── Abstractions/                 # Interfaces
│   ├── Behaviors/                    # MediatR pipeline behaviors
│   ├── Validators/                   # FluentValidation rules
│   └── DependencyInjection.cs
│
├── YGZ.ServiceName.Domain/           # Domain Layer
│   ├── Entities/                     # Domain entities
│   ├── ValueObjects/                 # Value objects (DDD)
│   ├── Events/                       # Domain events
│   ├── Errors/                       # Domain error definitions
│   └── Core/                         # Base classes
│
└── YGZ.ServiceName.Infrastructure/   # Infrastructure Layer
    ├── Persistence/
    │   ├── Data/                     # DbContext
    │   ├── Migrations/               # EF migrations
    │   ├── Repositories/             # Repository implementations
    │   └── Configurations/           # Entity configurations
    ├── Services/                     # External service integrations
    ├── Settings/                     # Configuration classes
    └── DependencyInjection.cs
```

### Backend Conventions

-   **Namespace pattern**: `YGZ.ServiceName.Layer.Feature`
-   **File naming**: PascalCase for all C# files
-   **One class per file** (except nested classes)
-   **Interfaces**: Prefix with `I` (e.g., `IUserRepository`)
-   **Commands**: Suffix with `Command` (e.g., `CreateUserCommand`)
-   **Queries**: Suffix with `Query` (e.g., `GetUserQuery`)
-   **Handlers**: Suffix with `Handler` (e.g., `CreateUserCommandHandler`)
-   **Validators**: Suffix with `Validator` (e.g., `CreateUserCommandValidator`)

### Identity Service as Template

The **Identity service** (`Services/Identity/`) is the reference implementation. When creating new services or features, follow its patterns:

-   Layer organization and dependencies
-   CQRS command/query structure
-   Result pattern for error handling
-   FluentValidation setup
-   Repository pattern implementation
-   Dependency injection registration

## Shared Code (`Services/BuildingBlocks/`)

```
BuildingBlocks/
├── YGZ.BuildingBlocks.Shared/
│   ├── Abstractions/          # CQRS interfaces (ICommand, IQuery)
│   ├── Behaviors/             # MediatR pipeline behaviors
│   ├── Contracts/             # Shared DTOs and responses
│   ├── Core/                  # Base entity, aggregate root
│   ├── Extensions/            # Common service extensions
│   ├── Primitives/            # Result pattern, Error types
│   └── Utilities/             # Helper functions
│
└── YGZ.BuildingBlocks.Messaging/
    ├── Events/                # Integration events
    └── Configuration/         # MassTransit setup
```

### Using BuildingBlocks

-   Reference `YGZ.BuildingBlocks.Shared` in all service layers
-   Use `Result<T>` pattern instead of throwing exceptions
-   Implement `ICommand<TResponse>` for write operations
-   Implement `IQuery<TResponse>` for read operations
-   Inherit from `Entity<TId>` for domain entities

## API Gateway (`Gateways/YGZ.Gateways.Yarp/`)

```
YGZ.Gateways.Yarp/
├── Extensions/            # YARP configuration
├── appsettings.json      # Route configuration
└── Program.cs
```

Routes requests to microservices based on path patterns.

## Blockchain (`blockchain/payment/`)

```
blockchain/payment/
├── programs/payment/      # Rust Anchor program
│   └── src/
│       └── lib.rs        # Smart contract logic
├── app/                  # Next.js frontend for testing
├── tests/                # TypeScript tests
├── migrations/           # Deployment scripts
└── Anchor.toml          # Anchor configuration
```

## Documentation (`docs/`)

```
docs/
├── databases/            # Database schemas and diagrams
├── usecases/            # PlantUML use case diagrams
└── seed/                # Data seeding guides
```

## Key File Locations

**Environment configs**: `.env`, `.env.example` (root and per app)
**Docker configs**: `docker-compose.yml`, `docker-compose.override.yml`
**Solution file**: `YGZ.CA.Microservices.sln` (root)
**Service solution**: `Services/Services.sln` (backend only)
**Nginx configs**: `provision/webserver/nginx.prod.conf`
**Jenkins pipelines**: `provision/cd/jenkinsfile`, `provision/client/client.jenkinsfile`

## Naming Conventions Summary

**Frontend**:

-   Files: kebab-case (`user-profile.tsx`)
-   Components: PascalCase (`UserProfile`)
-   Hooks: camelCase with `use` prefix (`useUserData`)
-   Types/Interfaces: PascalCase (`UserProfile`)

**Backend**:

-   Files: PascalCase (`UserController.cs`)
-   Classes: PascalCase (`UserController`)
-   Interfaces: PascalCase with `I` prefix (`IUserRepository`)
-   Methods: PascalCase (`GetUserByIdAsync`)
-   Private fields: camelCase with `_` prefix (`_userRepository`)
-   Constants: PascalCase or UPPER_CASE

**Database**:

-   Tables: PascalCase (`Users`, `OrderItems`)
-   Columns: PascalCase (`FirstName`, `CreatedAt`)
-   Foreign keys: `{Table}Id` (`UserId`, `OrderId`)
