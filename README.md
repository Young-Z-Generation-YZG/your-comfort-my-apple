# Your Comfort My Apple - Graduation Project (KLTN)

<div align="center">
  <img src="https://img.shields.io/badge/University-HCMUTE-blue.svg" alt="University" />
  <img src="https://img.shields.io/badge/Specialization-Software%20Engineering-green.svg" alt="Specialization" />
  <img src="https://img.shields.io/badge/Academic%20Year-2021--2025-orange.svg" alt="Academic Year" />
  <img src="https://img.shields.io/badge/Grade-A%2B-red.svg" alt="Grade" />
</div>

---

## 📖 Graduation Thesis (KLTN)
**Title:** Building an electronics e-commerce website integrated with Blockchain payment and Multi-tenant management to optimize system revenue.

**Vietnamese:** Xây dựng website bán thiết bị điện tử tích hợp thanh toán bằng Blockchain, hỗ trợ quản lý đa chi nhánh Multi-tenant giúp tối ưu doanh thu hệ thống.

**Final Grade:** **A+** (High Distinction)

---

## 🌟 Project Overview

**Your Comfort My Apple (YGZ)** is an enterprise-grade e-commerce platform designed to tackle complex business requirements such as multi-branch management and modern decentralized payments. The system is built on a high-performance **Microservices Architecture** using **.NET 8** and **Next.js 14**, integrated with the **Solana Blockchain** for secure cryptocurrency transactions.

---

## 📸 Screenshots

### 🏠 Client Storefront
| Home Page | Shop Page | Product Details |
| :---: | :---: | :---: |
| ![Home Page](docs/README_IMAGES/client/home_page.png) | ![Shop Page](docs/README_IMAGES/client/main_sell_page.png) | ![Product Details](docs/README_IMAGES/client/product_details_page.png) |

| Promotion Event 1 | Promotion Event 2 |
| :---: | :---: |
| ![Event 1](docs/README_IMAGES/client/event_page_1.png) | ![Event 2](docs/README_IMAGES/client/event_page_2.png) |

| Shopping Cart | Checkout Details | Blockchain Payment |
| :---: | :---: | :---: |
| ![Basket](docs/README_IMAGES/client/basket_page.png) | ![Checkout 1](docs/README_IMAGES/client/checkout_page_1.png) | ![Solana Payment](docs/README_IMAGES/client/checkout_page_2_blockchain_payment.png) |

### 📊 Admin Portal
| Dashboard | Revenue Analytics |
| :---: | :---: |
| ![Admin Dashboard](docs/README_IMAGES/admin/admin_dashboard.png) | ![Revenue Chart](docs/README_IMAGES/admin/revenue_chart.png) |

| Order Management | Human Resource (HRM) | Warehouse Management |
| :---: | :---: | :---: |
| ![Online Orders](docs/README_IMAGES/admin/online_orders_table.png) | ![HRM Table](docs/README_IMAGES/admin/hrm_table.png) | ![Warehouse Table](docs/README_IMAGES/admin/warehouse_table.png) |

### 🪵 Logging & 🔍 Tracking
| Structured Logging (Seq) | Distributed Tracking (Jaeger) |
| :---: | :---: |
| ![Logging](docs/README_IMAGES/logging/microservices_logging.png) | ![Tracking](docs/README_IMAGES/tracking/microservices_internal_request_tracking.png) |

---

## 🌐 Live Demo & Deployed Services

- **Storefront (Client):** [ybzone.io.vn](https://ybzone.io.vn/)
- **Admin Portal:** [admin.ybzone.io.vn](https://admin.ybzone.io.vn/)
- **Infrastructure & Observability:**
  - **Identity Server:** [keycloak.ybzone.io.vn](https://keycloak.ybzone.io.vn/)
  - **Structured Logging:** [logging.ybzone.io.vn](https://logging.ybzone.io.vn/)
  - **Distributed Tracking:** [tracking.ybzone.io.vn](https://tracking.ybzone.io.vn/)

---

### 🔑 Key Research Areas
- **Microservices Architecture:** Independently scalable services with isolated databases.
- **Domain-Driven Design (DDD):** Aligning software design with complex business logic.
- **Multi-Tenancy:** Database-level and application-level isolation for multi-branch/multi-business operations.
- **Blockchain Integration:** Implementing Solana/Anchor smart contracts for decentralized payments.
- **Clean Architecture:** Maintaining a highly maintainable and testable codebase.
- **Identity Server (Keycloak):** Centralized authentication and authorization (OIDC/OAuth2).

---

## 🏗️ Architecture & Core Concepts

The platform emphasizes scalability, maintainability, and loose coupling through several architectural patterns:

- **CQRS & MediatR:** Clear separation between Read (Query) and Write (Command) operations.
- **Event-Driven Communication:** Asynchronous messaging between services via **RabbitMQ** and **MassTransit**.
- **gRPC Services:** High-performance synchronous internal communication (e.g., Basket-to-Discount).
- **YARP API Gateway:** Advanced routing, load balancing, and cross-cutting concern handling.
- **Result Pattern:** Standardized operation results with error tracking.
- **Tenant Isolation:** Strategy to manage multiple branches efficiently with branch-specific revenue tracking.

---

## �️ Technology Stack

| Layer | Technologies |
| :--- | :--- |
| **Backend** | .NET 8, ASP.NET Core, Entity Framework Core, MongoDB Driver, MediatR, gRPC, Serilog, OpenTelemetry |
| **Frontend** | Next.js 14 (App Router), TypeScript, Redux Toolkit, RTK Query, Tailwind CSS, Framer Motion |
| **Databases** | PostgreSQL (Relational), MongoDB (Document), Redis (Caching) |
| **Security** | Keycloak, JWT, OIDC, RBAC |
| **Messaging** | RabbitMQ, MassTransit |
| **Blockchain** | Solana, Rust, Anchor Framework, `@solana/web3.js` |
| **DevOps** | Docker, Docker Compose, Nginx, Seq, Jaeger |

### 🛠️ Core Technologies & Library Purpose

#### **Backend (.NET Microservices)**
- **MediatR**: Implementation of the **CQRS** pattern to decouple the API Layer from Business Logic.
- **FluentValidation**: Centralized and expressive validation for all incoming requests.
- **Mapster**: High-performance object-to-object mapping between Entities and DTOs.
- **Entity Framework Core**: Advanced ORM for handling complex relational data in PostgreSQL.
- **MongoDB Driver**: High-performance interaction with Document-based data for the Catalog service.
- **MassTransit & RabbitMQ**: Orchestrates asynchronous **Event-Driven Messaging** for distributed consistency.
- **Keycloak**: Enterprise-grade **Identity Server** providing OIDC/OAuth2 authentication and fine-grained RBAC.
- **Serilog & Seq**: Provides **Structured Logging** with parsed parameters for efficient searching and monitoring.
- **OpenTelemetry & Jaeger**: Implements **Distributed Tracing** to track requests across multiple service boundaries.
- **YARP (Yet Another Reverse Proxy)**: A programmable **API Gateway** for routing, load balancing, and cross-cutting concerns.
- **Quartz.NET**: Handles complex background scheduling tasks, such as order lifecycle management.
- **Polly & Resilience**: Implements retry policies and circuit breakers for robust cross-service communication.

#### **Frontend (User Interfaces)**
- **Next.js 14 (App Router)**: Leverages Server Components and Optimized Routing for extreme production performance.
- **Redux Toolkit & RTK Query**: Manages global state and intelligently caches API responses to minimize network overhead.
- **Tailwind CSS & Shadcn UI**: Utility-first styling combined with accessible UI components for a premium look and feel.
- **Framer Motion**: Adds micro-interactions and smooth transitions to enhance the user experience.

#### **Blockchain & Payments**
- **Solana & Anchor**: High-throughput blockchain and framework for building secure **Smart Contracts**.
- **Rust**: Ensures safety and performance in the "Payment" on-chain logic.
- **`@solana/web3.js`**: Bridges the Frontend with the Solana network for transaction signing and processing.

---

## 🔥 Highlighted Features

- **Multi-Branch Management:** Specialized revenue analytics and inventory tracking per tenant.
- **Flexible Payments:** Support for **COD**, **VNPay**, **MoMo**, and **Solana (SOL)**.
- **Advanced Admin Tools:**
  - **User Impersonation:** Super admins can troubleshoot by viewing the system as a specific staff member.
  - **Tenant Switching:** Quick switching between branches for global management.
- **Product Lifecycle:** Comprehensive SKU management with variants (Color, Storage, Model).
- **SEO & UX:** Server-side rendering (SSR), optimized images, and fluid animations for a premium feel.


## �📂 Project Structure

```text
.
├── apps/                        # Frontend Applications
│   ├── client/                  # Customer-facing storefront
│   └── admin/                   # Back-office management portal
├── Services/                    # .NET Microservices
│   ├── Catalog/                 # Product Discovery (MongoDB)
│   ├── Basket/                  # Cart & Checkout Orchestration
│   ├── Discount/                # Promotion Engine (gRPC)
│   ├── Identity/                # Identity & Access Management
│   ├── Ordering/                # Lifecycle & Payment Processing
│   └── BuildingBlocks/          # Shared Libraries (Messaging, Event Bus)
├── Gateways/                    # API Routing
│   └── YGZ.Gateways.Yarp/       # Reverse Proxy
├── blockchain/                  # Smart Contracts (Rust/Anchor)
└── provision/                   # CLIs, CI/CD, SSL & Configuration
```

---

## 🚦 Getting Started

### Prerequisites
- Docker & Docker compose
- .NET 8 SDK
- Node.js (LTS)

### Local Deployment

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Young-Z-Generation-YZG/your-comfort-my-apple.git
   cd your-comfort-my-apple
   ```

2. **Generate Self-Signed SSL Certificates**:
   ```bash
   # Make the script executable and run it
   chmod +x provision/ssl/generate-ssl.sh
   ./provision/ssl/generate-ssl.sh
   ```

3. **Start Infrastructure** (optional):
   ```bash
   docker-compose up -d
   ```

4. **Run Services**:
   - Open `YGZ.CA.Microservices.sln` in Visual Studio.
   - Set "Docker compose" and run.

5. **Run Frontend**:
   ```bash
   cd apps/client
   npm install && npm run dev
   ```

---

## 🚀 Next Move (Upcoming Enhancements)

The roadmap for **Your Comfort My Apple** focuses on transitioning to a cloud-native ecosystem and deepening the microservices maturity:

- [ ] **Infrastructure Orchestration**: Migration to **Kubernetes (K8s)** for enterprise-grade container orchestration and high availability.
- [ ] **Infrastructure as Code (IaC)**: Leveraging **Ansible** for automated configuration management and repeatable environment provisioning.
- [ ] **Distributed Transactions**: Implementing the **Saga Pattern** (Orchestration/Choreography) using MassTransit to manage complex, multi-service business processes.
- [ ] **Advanced Search Service**: Implementation of a dedicated search microservice powered by **Elasticsearch** for fuzzy matching and real-time indexing.
- [ ] **Real-time Communication**: Adding a **SignalR-based** service for instant notifications, live order updates, and administrative alerts.
- [ ] **LGTM Observability Stack**: Migrating to a unified observability platform:
  - [ ] **Loki** (Logs), **Grafana** (Dashboards), **Tempo** (Traces), and **Prometheus** (Metrics).
- [ ] **Background Job Management**: Implementing robust, distributed workers for **Cron jobs, automated backups, and system cleanup**.
- [ ] **Object Storage**: Moving from Cloudinary to **MinIO** for self-hosted, S3-compatible high-performance object storage.

---

## 📄 License
This graduation project is developed by the YGZ Team and is provided for academic purposes.
