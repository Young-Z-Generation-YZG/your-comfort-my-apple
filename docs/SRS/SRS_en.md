# Software Requirements Specification (SRS)

## 1. Introduction

### 1.1 Purpose
The purpose of this document is to define the functional and non-functional requirements for the **"Your Comfort My Apple"** E-Commerce Platform. This system is a comprehensive online marketplace designed to sell electronic devices (specifically Apple products) with support for multi-tenant management and modern payment solutions including Blockchain.

### 1.2 Scope
The system is built using a **Microservices Architecture** to ensure scalability and maintainability. It serves multiple actors including Customers, Staff, Branch Managers, and System Administrators.

**Key Features:**
*   **Storefront:** Product browsing, searching, filtering, and detailed views.
*   **Shopping:** Real-time shopping cart, discount application, and checkout.
*   **Payments:** Multiple payment gateways including COD (Cash on Delivery), VNPay, MoMo, and **Solana Blockchain**.
*   **Identity:** Centralized authentication and authorization (Keycloak), supporting Customer and Administrative roles.
*   **Management:**
    *   **Multi-tenancy:** Management of multiple branches/tenants.
    *   **Inventory:** Per-branch inventory tracking.
    *   **Analytics:** Revenue reporting by branch and system-wide.
*   **Admin Tools:** Tenant creation, employee management, and user impersonation for support.

### 1.3 Definitions and Acronyms
*   **SRS:** Software Requirements Specification
*   **COD:** Cash on Delivery
*   **VNPay/MoMo:** Third-party payment gateways commonly used in Vietnam.
*   **Solana:** A high-performance blockchain platform used for cryptocurrency payments.
*   **Tenant:** A discrete branch or store entity within the system.
*   **SKU:** Stock Keeping Unit.

---

## 2. Overall Description

### 2.1 Product Perspective
The system operates as a distributed system composed of several core microservices:
*   **Identity Service:** User management and authentication (OpenID Connect/OAuth2 via Keycloak).
*   **Catalog Service:** Product management using MongoDB for flexibility.
*   **Basket Service:** Shopping cart management using Redis for high performance.
*   **Discount Service:** Coupon and promotion logic utilizing gRPC for internal communication.
*   **Ordering Service:** Order lifecycle management and payment processing.
*   **Frontend:** Two distinct Next.js applications:
    *   **Client App:** Public-facing store.
    *   **Admin App:** Back-office management portal.

### 2.2 User Characteristics
*   **Customer (Guest/Registered):** Browses products, manages cart, places orders, writes reviews.
*   **Staff/Branch Manager:** Manages orders and inventory for their specific assigned branch.
*   **System Administrator (Super Admin):** Manages all tenants/branches, views system-wide analytics, creates product catalogs, and manages system configuration.

### 2.3 Operating Environment
*   **Client Side:** Modern Web Browsers (Chrome, Firefox, Edge).
*   **Server Side:** Docker Containers hosting .NET 8 Microservices.
*   **Databases:** PostgreSQL, MongoDB, Redis.
*   **Blockchain Integration:** Solana Web3.js client and wallet extensions (Phantom/Solflare).

---

## 3. Specific Requirements

### 3.1 Functional Requirements

#### 3.1.1 Authentication & User Account
| ID | Feature | Description |
| :--- | :--- | :--- |
| **FR-AUTH-01** | Register | Users can create a new account details (Name, Email, Phone). Duplicate emails are rejected. |
| **FR-AUTH-02** | Login | Users can authenticate using Email and Password. Session management via JWT. |
| **FR-AUTH-03** | Email Verification | New accounts require email OTP verification before activation. |
| **FR-AUTH-04** | Password Recovery | Users can reset forgotten passwords via email link/OTP. |
| **FR-AUTH-05** | Logout | Users can securely terminate their session. |

#### 3.1.2 Product Catalog
| ID | Feature | Description |
| :--- | :--- | :--- |
| **FR-CAT-01** | Search Products | Users can search for products by keyword (Name, Model). |
| **FR-CAT-02** | Filter Products | Users can filter lists by Category, Price Range, Color, Storage, and Model. |
| **FR-CAT-03** | Product List | View products in a grid/list format with pagination. |
| **FR-CAT-04** | Product Details | View detailed info: Images, Description, Specs, Stock availability per branch. |
| **FR-CAT-05** | Product Reviews | Verified buyers can rate (1-5 stars) and review products. |

#### 3.1.3 Shopping Cart (Basket)
| ID | Feature | Description |
| :--- | :--- | :--- |
| **FR-CART-01** | Add to Cart | Add products with specific attributes (Color, Storage) to the basket. Checks real-time stock. |
| **FR-CART-02** | Update Cart | Modify quantities or remove items. Basket total recalculates automatically. |
| **FR-CART-03** | Apply Coupon | Enter valid promo codes to receive discounts on the cart total. |

#### 3.1.4 Ordering & Payments
| ID | Feature | Description |
| :--- | :--- | :--- |
| **FR-ORD-01** | Checkout | Process cart items into an order. Collect shipping address and contact info. |
| **FR-ORD-02** | Payment: COD | Pay via Cash on Delivery. |
| **FR-ORD-03** | Payment: VNPay | Pay online via VNPay gateway integration. |
| **FR-ORD-04** | Payment: Solana | Pay using SOL cryptocurrency via connected Blockchain wallet (Phantom/Solflare). |
| **FR-ORD-05** | Order Confirmation | Users must confirm orders within a specific timeframe (e.g., 30 mins) or they may be auto-confirmed/cancelled. |
| **FR-ORD-06** | Order Tracking | View order status history (Pending -> Confirmed -> Delivering -> Completed). |

#### 3.1.5 Administration & Management
| ID | Feature | Description |
| :--- | :--- | :--- |
| **FR-ADM-01** | Dashboard | View high-level metrics: Total Revenue, Orders, New User Stats. |
| **FR-ADM-02** | Tenant Management | **Super Admin Only.** Create and manage new Tenants (Branches/Warehouses) with unique subdomains. |
| **FR-ADM-03** | Revenue Analytics | View revenue reports filtered by Branch, Time Period (Month/Year), and compare branches. |
| **FR-ADM-04** | Inventory Management | Manage product stock levels specific to each branch/tenant. |
| **FR-ADM-05** | Order Management | View all orders, filter by status/payment method, and update order status (e.g., Update "Pending" to "Delivering"). |
| **FR-ADM-06** | Promotion Management | Create and manage discount events and coupons globally or per category/product. |
| **FR-ADM-07** | Staff Management | Create and manage staff accounts, assigning them to specific branches. |
| **FR-ADM-08** | Impersonation | **Super Admin Only.** Ability to "impersonate" another staff member to view the system from their perspective for debugging/support. |

### 3.2 Non-Functional Requirements
1.  **Performance:** The system should handle high concurrency. Searching and filtering should respond in under 2 seconds.
2.  **Scalability:** Microservices architecture allows independent scaling of high-load services (e.g., Catalog, Ordering).
3.  **Security:**
    *   All passwords must be hashed (BCrypt).
    *   API communications secured via HTTPS.
    *   Role-based Access Control (RBAC) enforced at the Gateway and Service level.
4.  **Reliability:** The system must ensure data consistency, especially for distributed transactions (e.g., stock reservation vs payment).
5.  **Multi-Tenancy:** Strict data isolation between tenants ensuring branch managers only see their own branch's data (unless Super Admin).

### 3.3 System Constraints
*   **Backend:** Must use .NET 8+.
*   **Database:** PostgreSQL for transactional data, MongoDB for Catalog, Redis for Caching.
*   **Frontend:** Next.js 14+ (App Router).
*   **Blockchain:** Solana Devnet for crypto payments.

---

## 4. Future Enhancements
*   **AI Chatbot:** Integration of a customer support chatbot (Dialogflow/Rasa).
*   **Advanced Search:** Implementation of ElasticSearch for fuzzy matching and faster retrieval.
*   **Smart Recommendations:** Machine learning based product recommendations based on user browsing history.
*   **Localization:** Multi-language support.
