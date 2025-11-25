# Product Overview

YGZ is a multi-tenant e-commerce platform built with microservices architecture. The system supports online shopping with product catalog, shopping cart, order processing, and payment integration including blockchain-based payments.

## Applications

**Client App** - Customer-facing e-commerce application for browsing products, managing shopping cart, and placing orders.

**Admin App** - Administrative dashboard for managing products, orders, users, and viewing analytics.

## Core Services

-   **Catalog** - Product and category management with reviews and promotions
-   **Basket** - Shopping cart operations and item management
-   **Discount** - Coupon validation and promotion rules
-   **Ordering** - Order processing, status tracking, and payment integration
-   **Identity** - User authentication and authorization via Keycloak

## Payment Options

-   Traditional payment gateway (VNPay)
-   Blockchain-based payment using Solana (Anchor framework)

## Key Features

-   Multi-tenant support with tenant isolation
-   Real-time notifications via SignalR
-   Event-driven architecture with RabbitMQ
-   Distributed caching with Redis
-   Centralized logging with Seq
-   Distributed tracing with Jaeger
