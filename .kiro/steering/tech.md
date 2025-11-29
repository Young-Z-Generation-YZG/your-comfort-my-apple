# Technology Stack

## Frontend

**Framework**: Next.js 14/15 (React)
**Language**: TypeScript
**Styling**: Tailwind CSS
**UI Components**: Radix UI primitives
**State Management**: Redux Toolkit with Redux Persist
**Forms**: React Hook Form with Zod validation
**HTTP Client**: Native fetch with gRPC support
**Icons**: Lucide React, React Icons
**Animations**: Framer Motion
**Image Management**: Cloudinary (next-cloudinary)
**Blockchain**: Solana Web3.js, Wallet Standard

### Frontend Development Commands

```bash
# Client app (port 3000)
cd apps/client
npm run dev          # Development server
npm run build        # Production build
npm run lint         # Run ESLint
npm run prettier:fix # Format code

# Admin app (port 3001)
cd apps/admin
npm run dev          # Development server
npm run build        # Production build
npm run storybook    # Component development
```

## Backend

**Framework**: .NET 8 (ASP.NET Core)
**Language**: C#
**Architecture**: Clean Architecture + DDD
**API Gateway**: YARP (Yet Another Reverse Proxy)
**Communication**: REST APIs, gRPC
**Validation**: FluentValidation
**Mapping**: AutoMapper
**Patterns**: CQRS with MediatR

### Backend Technologies

**ORM**: Entity Framework Core 8
**Databases**:

-   PostgreSQL (Identity, Basket, Discount, Ordering)
-   MongoDB (Catalog)
-   Redis (Distributed caching)

**Messaging**: RabbitMQ with MassTransit
**Authentication**: Keycloak (OAuth2/OpenID Connect)
**Logging**: Serilog with Seq
**Tracing**: OpenTelemetry with Jaeger
**Health Checks**: ASP.NET Core Health Checks
**API Documentation**: Swagger/OpenAPI

### Backend Development Commands

```bash
# Run all services with Docker Compose
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d

# Build specific service
dotnet build Services/Catalog/YGZ.Catalog.Api

# Run specific service
dotnet run --project Services/Catalog/YGZ.Catalog.Api

# Entity Framework migrations
dotnet ef migrations add InitialCreate -o Persistence/Migrations -p YGZ.Identity.Infrastructure -s YGZ.Identity.Api
dotnet ef database update -p YGZ.Identity.Infrastructure -s YGZ.Identity.Api

# Remove last migration (not applied)
dotnet ef migrations remove -p YGZ.Identity.Infrastructure -s YGZ.Identity.Api
```

## Blockchain

**Platform**: Solana
**Framework**: Anchor (Rust-based)
**Language**: Rust + TypeScript
**Testing**: Mocha, Chai

### Blockchain Commands

```bash
cd blockchain/payment

# Build Anchor program
anchor build

# Run tests
anchor test

# Deploy to devnet
anchor deploy --provider.cluster devnet
```

## DevOps

**Containerization**: Docker
**Orchestration**: Docker Compose
**Reverse Proxy**: Nginx (production)
**CI/CD**: Jenkins (configured in provision/)

### Docker Services

-   API Gateway (YARP)
-   Microservices (Catalog, Basket, Discount, Ordering, Identity)
-   PostgreSQL databases (per service)
-   MongoDB with replica set
-   Redis
-   RabbitMQ
-   Keycloak
-   Seq (logging)
-   Jaeger (tracing)

## Development Tools

**Code Quality**: ESLint, Prettier, EditorConfig
**Version Control**: Git
**IDE Support**: Visual Studio, VS Code, Cursor
**Package Managers**: npm (frontend), NuGet (.NET)

## Common Patterns

**Error Handling**: Result pattern (no exceptions for business logic)
**Validation**: FluentValidation pipeline behaviors
**API Responses**: Problem Details (RFC 7807)
**Logging**: Structured logging with correlation IDs
**Caching**: Redis distributed cache with cache-aside pattern
**Events**: Domain events + integration events
