# Software Requirements Specification (SRS)

## Document Information

-   **Project Name**: TLCN_ADMIN - E-Commerce Platform
-   **Version**: 1.0
-   **Date**: 2024-01-15
-   **Author**: Development Team
-   **Status**: Draft

---

## 1. Introduction

### 1.1 Purpose

This Software Requirements Specification (SRS) defines the functional and non-functional requirements for the TLCN_ADMIN E-Commerce Platform. The system is a full-featured online marketplace built with a microservices architecture, supporting multi-tenant operations, product catalog management, shopping cart functionality, order processing, and comprehensive administrative capabilities.

### 1.2 Scope

The system provides:

**In Scope:**

-   Customer-facing storefront for browsing, searching, and purchasing products
-   Shopping cart management with real-time synchronization
-   Multiple payment gateway integrations (COD, VNPay, MoMo, Blockchain/Solana)
-   Order lifecycle management (creation, payment, fulfillment, tracking)
-   Product catalog with categories, variants (SKUs), inventory management
-   User authentication and authorization via Keycloak
-   Review and rating system
-   Discount and promotion management
-   Multi-tenant architecture support
-   Administrative portal for staff and super admins
-   Real-time notifications and updates

**Out of Scope:**

-   Third-party logistics integration
-   Advanced analytics and business intelligence
-   Mobile native applications (web-based only)
-   Supplier/vendor management portal
-   Advanced inventory forecasting

### 1.3 Definitions, Acronyms, and Abbreviations

-   **SRS**: Software Requirements Specification
-   **API**: Application Programming Interface
-   **SKU**: Stock Keeping Unit
-   **COD**: Cash on Delivery
-   **VNPay**: Vietnamese Payment Gateway
-   **MoMo**: Mobile Money Payment Gateway
-   **gRPC**: Google Remote Procedure Call
-   **JWT**: JSON Web Token
-   **OAuth2**: Open Authorization 2.0
-   **YARP**: Yet Another Reverse Proxy
-   **RTK Query**: Redux Toolkit Query
-   **UI**: User Interface
-   **UX**: User Experience

### 1.4 References

-   IEEE 830-1998 Standard for Software Requirements Specifications
-   Keycloak Identity and Access Management Documentation
-   Next.js Framework Documentation
-   .NET Microservices Architecture Guidelines
-   Payment Gateway API Documentation (VNPay, MoMo, Solana)

---

## 2. Overall Description

### 2.1 Product Perspective

The TLCN_ADMIN platform is a standalone e-commerce system that integrates with:

**External Systems:**

-   **Keycloak**: Identity and Access Management (authentication, authorization, user management)
-   **Payment Gateways**: VNPay, MoMo, Solana Blockchain
-   **Cloudinary**: Image storage and management
-   **Email Service**: Order confirmations, notifications
-   **Message Broker**: Event-driven communication between services

**Internal Components:**

-   **API Gateway (YARP)**: Request routing and load balancing
-   **Microservices**: Catalog, Basket, Discount, Identity, Ordering
-   **Databases**: PostgreSQL (primary), MongoDB (where applicable)
-   **Frontend Applications**: Client (Next.js), Admin (Next.js)

### 2.2 Product Functions

#### 2.2.1 Customer-Facing Features

1. **User Management**

    - User registration and account creation
    - User authentication (login/logout)
    - Password recovery
    - Profile management
    - Shipping address management

2. **Product Discovery**

    - Browse product catalog
    - Search products by name, SKU, or attributes
    - Filter products by category, price, availability
    - View product details (images, specifications, reviews)
    - View product variants (color, storage, model)

3. **Shopping Cart**

    - Add items to cart
    - Update item quantities
    - Remove items from cart
    - Select/deselect items for checkout
    - Apply discount coupons
    - View cart total with tax calculations

4. **Checkout and Payment**

    - Review order summary
    - Select shipping address
    - Choose payment method (COD, VNPay, MoMo, Blockchain)
    - Process payment transactions
    - Receive order confirmation

5. **Order Management**

    - View order history
    - View order details
    - Track order status
    - Confirm order delivery
    - Cancel orders (within allowed timeframe)

6. **Reviews and Ratings**
    - Write product reviews
    - Update existing reviews
    - Delete reviews
    - View product ratings and reviews

#### 2.2.2 Administrative Features

1. **Dashboard**

    - View revenue statistics
    - Filter revenue by date range
    - View key performance indicators

2. **Product Management**

    - Create, update, delete products
    - Manage product categories
    - Manage product variants (SKUs)
    - Manage inventory levels
    - Upload product images

3. **Order Management**

    - View all orders
    - Filter orders by status, date, customer
    - View order details
    - Update order status
    - Process order fulfillment

4. **Customer Management**

    - View customer list
    - Filter customers
    - View customer details
    - View customer order history

5. **Staff Management** (Admin/Super Admin)

    - View staff list
    - Add new staff members
    - Update staff information
    - Assign roles and permissions
    - Impersonate staff (Super Admin)

6. **Promotion Management**

    - Create and manage events/promotions
    - Create and manage discount coupons
    - Apply promotions to products/categories

7. **Tenant Management** (Super Admin)
    - Create and manage tenants
    - View tenant details
    - Update tenant information
    - Switch between tenants

### 2.3 User Characteristics

#### 2.3.1 End Users (Customers)

-   **Technical Skills**: Basic web browsing, online shopping experience
-   **Device Access**: Desktop, tablet, mobile devices
-   **Language**: Vietnamese (primary), English (secondary)
-   **Age Range**: 18-65 years

#### 2.3.2 Staff Users

-   **Technical Skills**: Basic computer literacy, familiarity with admin interfaces
-   **Roles**: Order processing, customer service, inventory management
-   **Access**: Limited to assigned functions

#### 2.3.3 Administrators

-   **Technical Skills**: Advanced, system administration experience
-   **Roles**: Full system access, staff management, configuration
-   **Access**: All administrative functions

#### 2.3.4 Super Administrators

-   **Technical Skills**: Expert level, system architecture understanding
-   **Roles**: Multi-tenant management, system-wide configuration
-   **Access**: All functions including tenant management and staff impersonation

### 2.4 Operating Environment

#### 2.4.1 Frontend Applications

-   **Framework**: Next.js 14+ (React 18+)
-   **Browsers**: Chrome, Firefox, Safari, Edge (latest 2 versions)
-   **Responsive Design**: Mobile, tablet, desktop
-   **State Management**: Redux Toolkit with RTK Query
-   **UI Components**: Radix UI, Tailwind CSS

#### 2.4.2 Backend Services

-   **Runtime**: .NET 8+
-   **Architecture**: Microservices
-   **API Style**: RESTful APIs, gRPC (for Discount service)
-   **Gateway**: YARP (Yet Another Reverse Proxy)
-   **Identity Provider**: Keycloak

#### 2.4.3 Infrastructure

-   **Operating System**: Linux/Windows Server
-   **Databases**: PostgreSQL 12+, MongoDB (where applicable)
-   **Containerization**: Docker, Docker Compose
-   **Message Broker**: RabbitMQ or similar
-   **Image Storage**: Cloudinary

#### 2.4.4 Development Environment

-   **Version Control**: Git
-   **CI/CD**: Jenkins (based on provision files)
-   **Package Management**: npm/yarn (frontend), NuGet (backend)

---

## 3. Specific Requirements

### 3.1 Functional Requirements

#### 3.1.1 User Authentication and Authorization

**FR-1: User Registration**

-   **FR-1.1**: System shall allow users to register with email, password, and personal information
-   **FR-1.2**: System shall validate email format and uniqueness
-   **FR-1.3**: System shall enforce password policy (minimum 8 characters, complexity requirements)
-   **FR-1.4**: System shall send email verification upon registration
-   **FR-1.5**: System shall integrate with Keycloak for user creation

**FR-2: User Authentication**

-   **FR-2.1**: System shall allow users to login with email and password
-   **FR-2.2**: System shall support OAuth2/OpenID Connect via Keycloak
-   **FR-2.3**: System shall implement session management with JWT tokens
-   **FR-2.4**: System shall lock account after 5 failed login attempts for 30 minutes
-   **FR-2.5**: System shall support "Remember Me" functionality
-   **FR-2.6**: System shall support logout functionality

**FR-3: Password Recovery**

-   **FR-3.1**: System shall allow users to request password reset via email
-   **FR-3.2**: System shall generate secure reset tokens with expiration
-   **FR-3.3**: System shall allow users to set new password using reset token

**FR-4: Role-Based Access Control**

-   **FR-4.1**: System shall support roles: Guest, User, Staff, Admin, Super Admin
-   **FR-4.2**: System shall enforce role-based permissions for all operations
-   **FR-4.3**: System shall allow Super Admin to impersonate staff users
-   **FR-4.4**: System shall support multi-tenant isolation

#### 3.1.2 Product Catalog Management

**FR-5: Product Browsing**

-   **FR-5.1**: System shall display products with images, title, price, availability
-   **FR-5.2**: System shall support pagination (configurable items per page)
-   **FR-5.3**: System shall display product categories in navigation
-   **FR-5.4**: System shall show featured/newest/popular products

**FR-6: Product Search**

-   **FR-6.1**: System shall allow searching by product name, SKU, or description
-   **FR-6.2**: System shall return search results within 2 seconds
-   **FR-6.3**: System shall highlight search terms in results
-   **FR-6.4**: System shall support autocomplete suggestions

**FR-7: Product Filtering**

-   **FR-7.1**: System shall allow filtering by category
-   **FR-7.2**: System shall allow filtering by price range
-   **FR-7.3**: System shall allow filtering by availability (in stock/out of stock)
-   **FR-7.4**: System shall allow filtering by product attributes (color, storage, model)
-   **FR-7.5**: System shall support multiple filter combinations

**FR-8: Product Details**

-   **FR-8.1**: System shall display complete product information (images, specifications, description)
-   **FR-8.2**: System shall display product variants (SKUs) with different attributes
-   **FR-8.3**: System shall display current inventory levels
-   **FR-8.4**: System shall display product reviews and ratings
-   **FR-8.5**: System shall show related/recommended products

**FR-9: Product Management (Admin)**

-   **FR-9.1**: System shall allow admins to create products with all required fields
-   **FR-9.2**: System shall allow admins to update product information
-   **FR-9.3**: System shall allow admins to delete products (with validation)
-   **FR-9.4**: System shall allow admins to manage product categories
-   **FR-9.5**: System shall allow admins to manage SKUs and inventory
-   **FR-9.6**: System shall allow admins to upload product images to Cloudinary

#### 3.1.3 Shopping Cart Management

**FR-10: Cart Operations**

-   **FR-10.1**: System shall allow users to add products to cart
-   **FR-10.2**: System shall allow users to update item quantities (1 to available stock)
-   **FR-10.3**: System shall allow users to remove items from cart
-   **FR-10.4**: System shall allow users to select/deselect items for checkout
-   **FR-10.5**: System shall persist cart for authenticated users across sessions
-   **FR-10.6**: System shall store cart in Redux for unauthenticated users (browser memory)

**FR-11: Cart Synchronization**

-   **FR-11.1**: System shall automatically sync Redux cart with server for authenticated users
-   **FR-11.2**: System shall merge local cart with server cart on login
-   **FR-11.3**: System shall handle cart conflicts (server takes precedence)
-   **FR-11.4**: System shall update cart totals in real-time

**FR-12: Cart Calculations**

-   **FR-12.1**: System shall calculate subtotal (sum of selected items)
-   **FR-12.2**: System shall apply discount coupons to selected items
-   **FR-12.3**: System shall calculate tax (if applicable)
-   **FR-12.4**: System shall calculate final total
-   **FR-12.5**: System shall display original price and discounted price when applicable

#### 3.1.4 Discount and Promotion Management

**FR-13: Coupon Application**

-   **FR-13.1**: System shall allow users to apply discount coupons to cart
-   **FR-13.2**: System shall validate coupon codes (validity, expiration, usage limits)
-   **FR-13.3**: System shall apply coupon only to selected items
-   **FR-13.4**: System shall remove coupon if no items are selected
-   **FR-13.5**: System shall display discount amount and updated total

**FR-14: Promotion Management (Admin)**

-   **FR-14.1**: System shall allow admins to create events/promotions
-   **FR-14.2**: System shall allow admins to create discount coupons
-   **FR-14.3**: System shall allow admins to set promotion validity periods
-   **FR-14.4**: System shall allow admins to set usage limits per user/coupon
-   **FR-14.5**: System shall allow admins to view and update promotions

#### 3.1.5 Checkout and Payment

**FR-15: Checkout Process**

-   **FR-15.1**: System shall require user authentication before checkout
-   **FR-15.2**: System shall display order summary (items, quantities, prices)
-   **FR-15.3**: System shall allow users to select shipping address
-   **FR-15.4**: System shall allow users to add new shipping address during checkout
-   **FR-15.5**: System shall display final total including discounts and tax
-   **FR-15.6**: System shall validate inventory availability before checkout

**FR-16: Payment Methods**

-   **FR-16.1**: System shall support Cash on Delivery (COD)
-   **FR-16.2**: System shall support VNPay payment gateway
-   **FR-16.3**: System shall support MoMo payment gateway
-   **FR-16.4**: System shall support Blockchain payment (Solana)
-   **FR-16.5**: System shall process payment transactions securely
-   **FR-16.6**: System shall handle payment failures gracefully

**FR-17: Order Creation**

-   **FR-17.1**: System shall create order after successful payment
-   **FR-17.2**: System shall generate unique order number
-   **FR-17.3**: System shall send order confirmation email
-   **FR-17.4**: System shall update inventory levels
-   **FR-17.5**: System shall clear cart after successful order

#### 3.1.6 Order Management

**FR-18: Order Viewing (Customer)**

-   **FR-18.1**: System shall display order history for logged-in users
-   **FR-18.2**: System shall show order status (Pending, Processing, Shipped, Delivered, Cancelled)
-   **FR-18.3**: System shall display order details (items, quantities, prices, shipping address)
-   **FR-18.4**: System shall show order timeline/status updates

**FR-19: Order Actions (Customer)**

-   **FR-19.1**: System shall allow users to confirm order delivery
-   **FR-19.2**: System shall allow users to cancel orders (within allowed timeframe)
-   **FR-19.3**: System shall allow users to write reviews for delivered items

**FR-20: Order Management (Admin)**

-   **FR-20.1**: System shall allow admins to view all orders
-   **FR-20.2**: System shall allow admins to filter orders by status, date, customer
-   **FR-20.3**: System shall allow admins to view order details
-   **FR-20.4**: System shall allow admins to update order status
-   **FR-20.5**: System shall send notifications when order status changes

#### 3.1.7 Review and Rating System

**FR-21: Review Management**

-   **FR-21.1**: System shall allow users to write reviews for delivered items
-   **FR-21.2**: System shall allow users to rate products (1-5 stars)
-   **FR-21.3**: System shall allow users to update their reviews
-   **FR-21.4**: System shall allow users to delete their reviews
-   **FR-21.5**: System shall display reviews on product detail pages
-   **FR-21.6**: System shall calculate and display average ratings

#### 3.1.8 Administrative Features

**FR-22: Dashboard**

-   **FR-22.1**: System shall display revenue statistics
-   **FR-22.2**: System shall allow filtering revenue by date range
-   **FR-22.3**: System shall display key performance indicators (orders, customers, products)

**FR-23: Staff Management**

-   **FR-23.1**: System shall allow admins to view staff list
-   **FR-23.2**: System shall allow admins to filter staff
-   **FR-23.3**: System shall allow admins to add new staff members
-   **FR-23.4**: System shall allow admins to view staff details
-   **FR-23.5**: System shall allow admins to update staff information
-   **FR-23.6**: System shall allow admins to assign roles to staff

**FR-24: Customer Management**

-   **FR-24.1**: System shall allow admins to view customer list
-   **FR-24.2**: System shall allow admins to filter customers
-   **FR-24.3**: System shall allow admins to view customer details
-   **FR-24.4**: System shall allow admins to view customer order history

**FR-25: Inventory Management**

-   **FR-25.1**: System shall allow admins to view inventory levels
-   **FR-25.2**: System shall allow admins to filter SKUs
-   **FR-25.3**: System shall display available stock quantities
-   **FR-25.4**: System shall update inventory when orders are placed

**FR-26: Tenant Management (Super Admin)**

-   **FR-26.1**: System shall allow super admins to view tenants
-   **FR-26.2**: System shall allow super admins to create tenants
-   **FR-26.3**: System shall allow super admins to view tenant details
-   **FR-26.4**: System shall allow super admins to update tenant information
-   **FR-26.5**: System shall allow super admins to switch between tenants

#### 3.1.9 Profile Management

**FR-27: User Profile**

-   **FR-27.1**: System shall allow users to view their profile
-   **FR-27.2**: System shall allow users to update profile information
-   **FR-27.3**: System shall allow users to change password
-   **FR-27.4**: System shall allow users to manage shipping addresses
-   **FR-27.5**: System shall allow users to set default shipping address

**FR-28: Address Management**

-   **FR-28.1**: System shall allow users to add shipping addresses
-   **FR-28.2**: System shall allow users to update shipping addresses
-   **FR-28.3**: System shall allow users to delete shipping addresses
-   **FR-28.4**: System shall allow users to set default address
-   **FR-28.5**: System shall validate address format

---

### 3.2 Non-Functional Requirements

#### 3.2.1 Performance Requirements

**NFR-1: Response Time**

-   **NFR-1.1**: Page load time shall not exceed 3 seconds under normal load
-   **NFR-1.2**: API response time shall not exceed 1 second for 95% of requests
-   **NFR-1.3**: Search results shall be returned within 2 seconds
-   **NFR-1.4**: Cart operations shall update UI within 500ms

**NFR-2: Throughput**

-   **NFR-2.1**: System shall support 1000 concurrent users
-   **NFR-2.2**: System shall handle 100 orders per minute during peak hours
-   **NFR-2.3**: System shall process 10,000 product catalog requests per minute

**NFR-3: Scalability**

-   **NFR-3.1**: System architecture shall support horizontal scaling
-   **NFR-3.2**: Database shall handle 1 million product records
-   **NFR-3.3**: System shall support multiple tenant instances

#### 3.2.2 Security Requirements

**NFR-4: Authentication and Authorization**

-   **NFR-4.1**: All API endpoints shall require authentication (except public catalog)
-   **NFR-4.2**: Passwords shall be hashed using bcrypt or similar
-   **NFR-4.3**: JWT tokens shall have expiration times
-   **NFR-4.4**: System shall implement role-based access control

**NFR-5: Data Protection**

-   **NFR-5.1**: All API communications shall use HTTPS
-   **NFR-5.2**: Payment information shall not be stored (use payment gateway tokenization)
-   **NFR-5.3**: System shall implement CSRF protection
-   **NFR-5.4**: System shall sanitize user inputs to prevent injection attacks
-   **NFR-5.5**: System shall implement rate limiting on API endpoints

**NFR-6: Data Privacy**

-   **NFR-6.1**: System shall comply with GDPR requirements (if applicable)
-   **NFR-6.2**: User data shall be encrypted at rest
-   **NFR-6.3**: System shall provide data export functionality for users

#### 3.2.3 Usability Requirements

**NFR-7: User Interface**

-   **NFR-7.1**: System shall be responsive (mobile, tablet, desktop)
-   **NFR-7.2**: System shall follow WCAG 2.1 Level AA accessibility standards
-   **NFR-7.3**: System shall support dark/light theme
-   **NFR-7.4**: Error messages shall be clear and actionable

**NFR-8: User Experience**

-   **NFR-8.1**: System shall provide intuitive navigation
-   **NFR-8.2**: System shall provide loading indicators for async operations
-   **NFR-8.3**: System shall provide confirmation dialogs for destructive actions
-   **NFR-8.4**: System shall support keyboard navigation

#### 3.2.4 Reliability Requirements

**NFR-9: Availability**

-   **NFR-9.1**: System uptime shall be 99.5%
-   **NFR-9.2**: System shall have automated daily backups
-   **NFR-9.3**: System shall recover from errors gracefully

**NFR-10: Fault Tolerance**

-   **NFR-10.1**: System shall handle service failures without complete system failure
-   **NFR-10.2**: System shall implement circuit breakers for external service calls
-   **NFR-10.3**: System shall log all errors for monitoring and debugging

#### 3.2.5 Maintainability Requirements

**NFR-11: Code Quality**

-   **NFR-11.1**: Code shall follow established coding standards
-   **NFR-11.2**: Code shall have unit test coverage of at least 70%
-   **NFR-11.3**: Code shall be documented with inline comments

**NFR-12: Monitoring and Logging**

-   **NFR-12.1**: System shall log all critical operations
-   **NFR-12.2**: System shall provide health check endpoints
-   **NFR-12.3**: System shall integrate with logging/monitoring tools (Seq, etc.)

#### 3.2.6 Compatibility Requirements

**NFR-13: Browser Compatibility**

-   **NFR-13.1**: System shall support Chrome (latest 2 versions)
-   **NFR-13.2**: System shall support Firefox (latest 2 versions)
-   **NFR-13.3**: System shall support Safari (latest 2 versions)
-   **NFR-13.4**: System shall support Edge (latest 2 versions)

**NFR-14: Platform Compatibility**

-   **NFR-14.1**: System shall run on Windows and Linux servers
-   **NFR-14.2**: System shall support Docker containerization
-   **NFR-14.3**: System shall support cloud deployment (AWS, Azure, GCP)

---

### 3.3 Interface Requirements

#### 3.3.1 User Interface

**UI-1: Client Application (Customer-Facing)**

-   Homepage with featured products and categories
-   Product listing page with filters and pagination
-   Product detail page with images, specifications, reviews
-   Shopping cart page with item management
-   Checkout page with multi-step form (Cart Review → Shipping → Payment → Confirmation)
-   User account page with profile, addresses, orders
-   Order history and order details pages

**UI-2: Admin Application (Back-Office)**

-   Dashboard with statistics and KPIs
-   Product management interface (CRUD operations)
-   Order management interface with status updates
-   Customer management interface
-   Staff management interface
-   Promotion/event management interface
-   Inventory management interface
-   Tenant management interface (Super Admin)

#### 3.3.2 Hardware Interface

-   None (web-based application, no direct hardware dependencies)

#### 3.3.3 Software Interface

**SI-1: Payment Gateway APIs**

-   **VNPay API**: RESTful API for payment processing
-   **MoMo API**: RESTful API for mobile money payments
-   **Solana Blockchain**: Web3 integration for cryptocurrency payments

**SI-2: Identity Provider (Keycloak)**

-   OAuth2/OpenID Connect protocol
-   User management APIs
-   Token management

**SI-3: Image Storage (Cloudinary)**

-   RESTful API for image upload and management
-   Image transformation and optimization

**SI-4: Email Service**

-   SMTP or API-based email service
-   Order confirmations, password resets, notifications

**SI-5: Message Broker**

-   RabbitMQ or similar for event-driven communication
-   Integration events between microservices

#### 3.3.4 Communication Interface

**CI-1: Client-Server Communication**

-   HTTPS protocol for all client-server communication
-   RESTful API endpoints
-   WebSocket or SignalR for real-time notifications

**CI-2: Inter-Service Communication**

-   RESTful APIs for synchronous communication
-   gRPC for Discount service
-   Message broker for asynchronous events
-   Service-to-service authentication

**CI-3: API Gateway (YARP)**

-   Request routing and load balancing
-   Authentication and authorization
-   Rate limiting
-   Request/response transformation

---

### 3.4 System Constraints

#### 3.4.1 Technical Constraints

-   Must use .NET 8+ for backend services
-   Must use Next.js 14+ for frontend applications
-   Must use PostgreSQL for primary data storage
-   Must integrate with Keycloak for identity management
-   Must support Docker containerization

#### 3.4.2 Business Constraints

-   Must comply with Vietnamese payment regulations (for VNPay, MoMo)
-   Must support multi-tenant architecture
-   Must maintain data isolation between tenants
-   Must support Vietnamese language (primary)

#### 3.4.3 Regulatory Constraints

-   Must comply with data protection regulations (GDPR if applicable)
-   Must implement secure payment processing (PCI DSS compliance considerations)
-   Must maintain audit logs for financial transactions

---

## 4. Appendices

### 4.1 Use Cases Summary

**Customer Use Cases:**

-   UC1: Register
-   UC2: Login
-   UC3: Password Recovery
-   UC4: Logout
-   UC5: View product list
-   UC6: Filter product
-   UC7: View product details
-   UC8: Search product
-   UC9: Add to cart
-   UC10: Manage cart
-   UC11: Apply coupon
-   UC12: Checkout
-   UC13: Payment with COD
-   UC14: Payment with VNPay
-   UC15: Payment with MoMo
-   UC16: Payment with Blockchain
-   UC17: View account
-   UC18: Update profile
-   UC19: Change password
-   UC20: View shipping addresses
-   UC21: Set default address
-   UC22: Add address
-   UC23: Update address
-   UC24: Delete address
-   UC25: View orders
-   UC26: View order details
-   UC27: Confirm order
-   UC28: Cancel order
-   UC29: Write review item
-   UC30: Update review item
-   UC31: Delete review item

**Admin Use Cases:**

-   UC32: View dashboard
-   UC33: Filter revenue
-   UC34: View online orders
-   UC35: View online order details
-   UC36: Update online order status
-   UC37: View tenants (Super Admin)
-   UC38: Create tenant (Super Admin)
-   UC39: View tenant details (Super Admin)
-   UC40: Update tenant (Super Admin)
-   UC41: View events
-   UC42: Create event
-   UC43: View event details
-   UC44: Update event
-   UC45: View categories
-   UC46: Create category
-   UC47: View category details
-   UC48: Update category
-   UC49: View staffs
-   UC50: Filter staff
-   UC51: Add staff
-   UC52: View staff details
-   UC53: Update staff
-   UC54: View products
-   UC55: Filter product
-   UC56: Create product
-   UC57: View product details
-   UC58: Update product
-   UC59: View inventory
-   UC60: Filter SKU
-   UC61: View customers
-   UC62: Filter customer
-   UC63: View customer details
-   UC64: View orders
-   UC65: Filter order
-   UC66: View order details
-   UC67: Impersonate staff (Super Admin)
-   UC68: Switch tenant (Super Admin)

### 4.2 Data Models

**User Entity:**

-   id (UUID)
-   email (string, unique)
-   password_hash (string)
-   full_name (string)
-   phone_number (string, optional)
-   created_at (timestamp)
-   updated_at (timestamp)

**Product Entity:**

-   id (UUID)
-   name (string)
-   description (text)
-   category_id (UUID, foreign key)
-   tenant_id (UUID, foreign key)
-   created_at (timestamp)
-   updated_at (timestamp)

**SKU Entity:**

-   id (UUID)
-   product_id (UUID, foreign key)
-   sku_code (string, unique)
-   color (string)
-   storage (string)
-   model (string)
-   price (decimal)
-   available_stock (integer)
-   created_at (timestamp)
-   updated_at (timestamp)

**Shopping Cart Entity:**

-   id (UUID)
-   user_id (UUID, foreign key)
-   cart_items (JSON array)
-   total_amount (decimal)
-   created_at (timestamp)
-   updated_at (timestamp)

**Order Entity:**

-   id (UUID)
-   order_number (string, unique)
-   user_id (UUID, foreign key)
-   tenant_id (UUID, foreign key)
-   status (enum: Pending, Processing, Shipped, Delivered, Cancelled)
-   total_amount (decimal)
-   shipping_address (JSON)
-   payment_method (enum: COD, VNPay, MoMo, Blockchain)
-   payment_status (enum: Pending, Paid, Failed)
-   created_at (timestamp)
-   updated_at (timestamp)

**Order Item Entity:**

-   id (UUID)
-   order_id (UUID, foreign key)
-   sku_id (UUID, foreign key)
-   quantity (integer)
-   unit_price (decimal)
-   discount_amount (decimal)
-   total_price (decimal)

**Review Entity:**

-   id (UUID)
-   user_id (UUID, foreign key)
-   order_item_id (UUID, foreign key)
-   rating (integer, 1-5)
-   comment (text)
-   created_at (timestamp)
-   updated_at (timestamp)

**Tenant Entity:**

-   id (UUID)
-   name (string)
-   domain (string, unique)
-   is_active (boolean)
-   created_at (timestamp)
-   updated_at (timestamp)

**Coupon Entity:**

-   id (UUID)
-   code (string, unique)
-   discount_type (enum: Percentage, FixedAmount)
-   discount_value (decimal)
-   min_purchase_amount (decimal, optional)
-   max_discount_amount (decimal, optional)
-   valid_from (timestamp)
-   valid_to (timestamp)
-   usage_limit (integer, optional)
-   usage_count (integer)
-   is_active (boolean)

### 4.3 Architecture Overview

**Microservices:**

1. **Catalog Service**: Product catalog, categories, SKUs, inventory, reviews
2. **Basket Service**: Shopping cart management, cart persistence
3. **Discount Service**: Coupon management, discount calculation (gRPC)
4. **Identity Service**: User management, authentication (Keycloak integration)
5. **Ordering Service**: Order lifecycle, payment processing, order status

**Shared Building Blocks:**

-   Messaging: Integration events, event handlers
-   Shared: Common contracts, enums, utilities, abstractions

**Frontend Applications:**

-   Client App: Customer-facing storefront (Next.js)
-   Admin App: Back-office portal (Next.js)

**Infrastructure:**

-   API Gateway (YARP): Request routing, authentication
-   Databases: PostgreSQL (primary), MongoDB (where applicable)
-   Message Broker: Event-driven communication
-   Identity Provider: Keycloak

### 4.4 Technology Stack

**Frontend:**

-   Next.js 14+
-   React 18+
-   Redux Toolkit with RTK Query
-   TypeScript
-   Tailwind CSS
-   Radix UI
-   React Hook Form
-   Zod (validation)

**Backend:**

-   .NET 8+
-   ASP.NET Core
-   Entity Framework Core
-   gRPC (for Discount service)
-   Keycloak (Identity)
-   PostgreSQL
-   Docker

**Infrastructure:**

-   Docker & Docker Compose
-   YARP (API Gateway)
-   RabbitMQ (or similar message broker)
-   Cloudinary (image storage)
-   Seq (logging)

### 4.5 API Endpoints Summary

**Catalog Service:**

-   GET /api/products
-   GET /api/products/{id}
-   GET /api/categories
-   GET /api/skus
-   POST /api/reviews
-   gRPC: Discount lookup

**Basket Service:**

-   GET /api/basket
-   POST /api/basket
-   POST /api/basket/items
-   POST /api/basket/sync
-   POST /api/basket/checkout
-   DELETE /api/basket

**Discount Service:**

-   gRPC: ApplyCoupon, ValidateCoupon

**Identity Service:**

-   POST /api/auth/register
-   POST /api/auth/login
-   POST /api/auth/logout
-   POST /api/auth/refresh
-   GET /api/users/profile
-   PUT /api/users/profile

**Ordering Service:**

-   GET /api/orders
-   GET /api/orders/{id}
-   POST /api/orders
-   PUT /api/orders/{id}/status
-   POST /api/orders/{id}/confirm
-   POST /api/orders/{id}/cancel

---

## 5. Change Management

### 5.1 Version History

| Version | Date       | Author           | Changes              |
| ------- | ---------- | ---------------- | -------------------- |
| 1.0     | 2024-01-15 | Development Team | Initial SRS document |

### 5.2 Approval

-   **Product Owner**: [Name] - [Date]
-   **Technical Lead**: [Name] - [Date]
-   **Stakeholder**: [Name] - [Date]

---

## 6. Glossary

-   **SKU**: Stock Keeping Unit - A unique identifier for a specific product variant
-   **COD**: Cash on Delivery - Payment method where customer pays upon delivery
-   **JWT**: JSON Web Token - A compact token format for authentication
-   **OAuth2**: Open Authorization 2.0 - Authorization framework
-   **gRPC**: Google Remote Procedure Call - High-performance RPC framework
-   **YARP**: Yet Another Reverse Proxy - Microsoft's reverse proxy library
-   **RTK Query**: Redux Toolkit Query - Data fetching and caching library
-   **Multi-tenant**: Architecture where a single instance serves multiple tenants
-   **Circuit Breaker**: Design pattern to prevent cascading failures
-   **CSRF**: Cross-Site Request Forgery - Security attack type

---

**End of Document**
