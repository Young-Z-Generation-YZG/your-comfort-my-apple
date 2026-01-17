# Your Comfort My Apple (YGZ) - TLCN E-Commerce Platform

[![Microservices Architecture](https://img.shields.io/badge/Architecture-Clean%20%2B%20Microservices-blue.svg)](https://dotnet.microsoft.com/en-us/apps/aspnet/microservices)
[![Next.js](https://img.shields.io/badge/Frontend-Next.js%2014-black.svg)](https://nextjs.org/)
[![.NET 8](https://img.shields.io/badge/Backend-.NET%208-512bd4.svg)](https://dotnet.microsoft.com/)
[![Solana](https://img.shields.io/badge/Blockchain-Solana%20%2F%20Anchor-9945FF.svg)](https://solana.com/)
[![Docker](https://img.shields.io/badge/DevOps-Docker%20Compose-2496ed.svg)](https://www.docker.com/)

A enterprise-grade, multi-tenant microservices e-commerce platform built with .NET 8, Next.js 14, and Solana blockchain. This project was developed as a Capstone Project (TLCN) to demonstrate a robust, scalable, and secure online marketplace.

---

## 🏗️ Architecture Overview

The platform is built on a distributed microservices architecture, emphasizing scalability, maintainability, and loose coupling.

- **Clean Architecture & DDD**: Each service is partitioned into `Api`, `Application`, `Domain`, and `Infrastructure`, following Domain-Driven Design principles.
- **CQRS & MediatR**: Separation of read and write concerns using Command Query Responsibility Segregation.
- **Multi-Tenancy**: Built-in support for multi-tenant isolation, allowing multiple businesses to run on the same infrastructure.
- **Event-Driven Messaging**: Asynchronous communication between services using **RabbitMQ** and **MassTransit**.
- **gRPC**: High-performance, low-latency synchronous communication between services (e.g., Basket to Discount).
- **API Gateway**: Leverages **YARP** for routing, load balancing, and cross-cutting concerns.
- **Identity & Security**: Integrated with **Keycloak** for OIDC/OAuth2 authentication and role-based access control (RBAC).

---

## 🚀 Tech Stack

### Backend (.NET Microservices)
- **Framework**: .NET 8
- **Messaging**: RabbitMQ, MassTransit
- **Databases**: PostgreSQL (Primary/Relational), MongoDB (Catalog/Document), Redis (Distributed Caching)
- **Communication**: gRPC, REST
- **Observability**: Seq, OpenTelemetry, Jaeger
- **Auth**: Keycloak

### Frontend (User Interfaces)
- **Framework**: Next.js 14 (App Router)
- **State Management**: Redux Toolkit & RTK Query
- **Styling**: Tailwind CSS, Shadcn UI (Radix UI)
- **Web3**: Solana Wallet Adapter, `@solana/web3.js`

### Blockchain & Payments
- **Smart Contracts**: Rust (Anchor Framework) on Solana
- **Traditional Payments**: Integrations for **VNPay**, **MoMo**, and **COD**.

---

## 📂 Project Structure

```text
.
├── apps/                        # Frontend Applications
│   ├── client/                  # Customer-facing storefront (Next.js)
│   └── admin/                   # Back-office portal (Next.js)
├── Services/                    # Backend Microservices
│   ├── Catalog/                 # Product management & Discovery (MongoDB)
│   ├── Basket/                  # Cart management & Checkout orchestration
│   ├── Discount/                # Coupon & Promotion engine (gRPC)
│   ├── Identity/                # Custom user management & Token service
│   ├── Ordering/                # Order processing & Lifecycle management
│   └── BuildingBlocks/          # Shared libraries (Messaging, Shared Primitives)
├── Gateways/                    # API Gateways
│   └── YGZ.Gateways.Yarp/       # YARP-based Reverse Proxy
├── blockchain/                  # Decentralized Components
│   └── payment/                 # Solana/Anchor smart contracts
├── provision/                   # DevOps, Infrastructure & Nginx configs
└── docker-compose.yml           # Local orchestration setup
```

---

## 🚦 Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js (LTS)](https://nodejs.org/)
- [Solana CLI](https://docs.solana.com/cli/install-solana-cli-tools) & [Anchor](https://www.anchor-lang.com/docs/installation)

### Local Development Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Young-Z-Generation-YZG/your-comfort-my-apple.git
   cd your-comfort-my-apple
   ```

2. **Spin up infrastructure**:
   ```bash
   docker-compose up -d
   ```

3. **Run Backend (Visual Studio/Rider)**:
   Open `YGZ.CA.Microservices.sln` and start the services using the "Multiple Startup Projects" profile.

4. **Run Frontend**:
   ```bash
   # In apps/client or apps/admin
   npm install
   npm run dev
   ```

---

## 📜 Logging Standards

The project follows a strict structured logging pattern for observability.

| Context | Pattern |
| :--- | :--- |
| **CommandHandler** | `:::[CommandHandler:Name]::: ...` |
| **gRPC Calls** | `===[CommandHandler:Name][gRPC:Service][Method:Name]=== ...` |
| **Integration Events** | `###[CommandHandler:Name][IntegrationEvent:EventName]### ...` |
| **Domain Events** | `::::[DomainEventHandler:Name]:::: ...` |

---

## 🤝 Contributing & Standards

- **CQRS**: Use MediatR for all business logic flows.
- **Validation**: Use FluentValidation for request validation.
- **Mapping**: Use AutoMapper for DTO transformations.
- **Database**: Use Entity Framework Core for PostgreSQL and MongoDB Driver for Catalog.

---

## 📄 License
This project is licensed under the MIT License.
