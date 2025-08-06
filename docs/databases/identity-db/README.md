# Identity Database Schema

## Overview
The Identity database uses **PostgreSQL** and implements ASP.NET Core Identity framework for user authentication and authorization. The database is designed to support comprehensive user management, role-based access control, and profile management for the TLCN_Microservices platform.

## Database Architecture

### Core Identity Tables
The database follows the standard ASP.NET Core Identity schema with custom extensions for e-commerce functionality.

## Table Definitions

### 1. **Users Table** - Core User Management
```sql
CREATE TABLE public."Users" (
    "Id" text NOT NULL,
    "UserName" varchar(256) NULL,
    "NormalizedUserName" varchar(256) NULL,
    "Email" varchar(256) NULL,
    "NormalizedEmail" varchar(256) NULL,
    "EmailConfirmed" bool NOT NULL,
    "PasswordHash" text NULL,
    "SecurityStamp" text NULL,
    "ConcurrencyStamp" text NULL,   
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" bool NOT NULL,
    "TwoFactorEnabled" bool NOT NULL,
    "LockoutEnd" timestamptz NULL,
    "LockoutEnabled" bool NOT NULL,
    "AccessFailedCount" int4 NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);
```

#### Key Features:
- **Primary Key**: Text-based ID (supports GUID strings)
- **Email Management**: Normalized email for case-insensitive lookups
- **Security Features**: Password hashing, security stamps, concurrency control
- **Account Protection**: Two-factor authentication, lockout mechanisms
- **Phone Verification**: Phone number confirmation system

#### Indexes:
- `EmailIndex`: Optimized email lookups
- `UserNameIndex`: Unique username lookups

### 2. **Profiles Table** - Extended User Information
```sql
CREATE TABLE public."Profiles" (
    "Id" uuid NOT NULL,
    "FirstName" text NOT NULL,
    "LastName" text NOT NULL,
    "BirthDay" timestamptz NOT NULL,
    "Gender" text NOT NULL,
    "ImageId" text DEFAULT ''::text NULL,
    "ImageUrl" text DEFAULT ''::text NULL,
    "UserId" text NOT NULL,
    "UpdatedAt" timestamptz NOT NULL,
    CONSTRAINT "PK_Profiles" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Profiles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE
);
```

#### Key Features:
- **One-to-One Relationship**: Each user has exactly one profile
- **Personal Information**: Name, birth date, gender
- **Image Management**: Profile picture with Cloudinary integration
- **Audit Trail**: UpdatedAt timestamp for tracking changes
- **Cascade Delete**: Profile deleted when user is deleted

#### Indexes:
- `IX_Profiles_UserId`: Unique index ensuring one profile per user

### 3. **ShippingAddresses Table** - E-commerce Address Management
```sql
CREATE TABLE public."ShippingAddresses" (
    "Id" uuid NOT NULL,
    "Label" text NOT NULL,
    "ContactName" text NOT NULL,
    "ContactPhoneNumber" text NOT NULL,
    "AddressLine" text NOT NULL,
    "AddressDistrict" text NOT NULL,
    "AddressProvince" text NOT NULL,
    "AddressCountry" text NOT NULL,
    "IsDefault" bool NOT NULL,
    "UserId" text NOT NULL,
    "UpdatedAt" timestamptz NOT NULL,
    CONSTRAINT "PK_ShippingAddresses" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ShippingAddresses_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE
);
```

#### Key Features:
- **Multiple Addresses**: Users can have multiple shipping addresses
- **Address Components**: Structured address fields for international shipping
- **Default Address**: IsDefault flag for primary shipping address
- **Contact Information**: Name and phone for delivery coordination
- **Cascade Delete**: Addresses deleted when user is deleted

#### Indexes:
- `IX_ShippingAddresses_UserId`: Optimized lookups by user

### 4. **Roles Table** - Role-Based Access Control
```sql
CREATE TABLE public."Roles" (
    "Id" text NOT NULL,
    "Name" varchar(256) NULL,
    "NormalizedName" varchar(256) NULL,
    "ConcurrencyStamp" text NULL,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);
```

#### Key Features:
- **Role Management**: Centralized role definitions
- **Normalized Names**: Case-insensitive role name lookups
- **Concurrency Control**: Optimistic locking for role updates

#### Indexes:
- `RoleNameIndex`: Unique index for role name lookups

### 5. **UserRoles Table** - Many-to-Many User-Role Relationship
```sql
CREATE TABLE public."UserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_UserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."Roles"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE
);
```

#### Key Features:
- **Many-to-Many**: Users can have multiple roles, roles can have multiple users
- **Composite Primary Key**: Ensures unique user-role combinations
- **Cascade Delete**: User-role assignments deleted when user or role is deleted

#### Indexes:
- `IX_UserRoles_RoleId`: Optimized role-based queries

### 6. **UserClaims Table** - User-Specific Claims
```sql
CREATE TABLE public."UserClaims" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "UserId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_UserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE
);
```

#### Key Features:
- **Flexible Claims**: Custom user attributes and permissions
- **Key-Value Storage**: ClaimType and ClaimValue pairs
- **Identity Integration**: Standard ASP.NET Core Identity claims
- **Auto-Increment ID**: Unique identifier for each claim

#### Indexes:
- `IX_UserClaims_UserId`: Optimized user claim lookups

### 7. **RoleClaims Table** - Role-Specific Claims
```sql
CREATE TABLE public."RoleClaims" (
    "Id" int4 GENERATED BY DEFAULT AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
    "RoleId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_RoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RoleClaims_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."Roles"("Id") ON DELETE CASCADE
);
```

#### Key Features:
- **Role-Based Claims**: Permissions and attributes assigned to roles
- **Inheritance**: Users inherit claims from their roles
- **Flexible Permissions**: Custom permission system

#### Indexes:
- `IX_RoleClaims_RoleId`: Optimized role claim lookups

### 8. **UserLogins Table** - External Authentication
```sql
CREATE TABLE public."UserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_UserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_UserLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE
);
```

#### Key Features:
- **External Providers**: Google, Facebook, Microsoft, etc.
- **Provider-Specific Keys**: Unique identifiers from external providers
- **Multiple Providers**: Users can link multiple external accounts
- **Composite Primary Key**: LoginProvider + ProviderKey combination

#### Indexes:
- `IX_UserLogins_UserId`: Optimized user login lookups

### 9. **UserTokens Table** - Token Management
```sql
CREATE TABLE public."UserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_UserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_UserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE
);
```

#### Key Features:
- **Token Storage**: Refresh tokens, two-factor codes, etc.
- **Provider-Specific**: Tokens organized by login provider
- **Token Types**: Different token types (Name field)
- **Composite Primary Key**: UserId + LoginProvider + Name

## Relationships

### 1. **User-Profile Relationship**
- **Type**: One-to-One
- **Cardinality**: 1 User ↔ 1 Profile
- **Constraint**: Unique index on UserId in Profiles table

### 2. **User-ShippingAddresses Relationship**
- **Type**: One-to-Many
- **Cardinality**: 1 User ↔ Many ShippingAddresses
- **Constraint**: Foreign key with cascade delete

### 3. **User-Roles Relationship**
- **Type**: Many-to-Many
- **Cardinality**: Many Users ↔ Many Roles
- **Junction Table**: UserRoles table

### 4. **User-Claims Relationship**
- **Type**: One-to-Many
- **Cardinality**: 1 User ↔ Many UserClaims
- **Constraint**: Foreign key with cascade delete

### 5. **Role-Claims Relationship**
- **Type**: One-to-Many
- **Cardinality**: 1 Role ↔ Many RoleClaims
- **Constraint**: Foreign key with cascade delete

## Security Features

### 1. **Password Security**
- **Hashing**: PasswordHash field stores bcrypt/scrypt hashes
- **Security Stamp**: Invalidates tokens when password changes
- **Concurrency Stamp**: Optimistic locking for user updates

### 2. **Account Protection**
- **Two-Factor Authentication**: TwoFactorEnabled flag
- **Account Lockout**: LockoutEnd and AccessFailedCount
- **Email Confirmation**: EmailConfirmed flag
- **Phone Confirmation**: PhoneNumberConfirmed flag

### 3. **External Authentication**
- **Multiple Providers**: Google, Facebook, Microsoft, etc.
- **Provider Keys**: Secure storage of external provider identifiers
- **Token Management**: Refresh tokens and access tokens

## Data Access Patterns

### 1. **User Lookup Patterns**
```sql
-- Email-based lookup
SELECT * FROM "Users" WHERE "NormalizedEmail" = @email

-- Username-based lookup
SELECT * FROM "Users" WHERE "NormalizedUserName" = @username

-- User with profile
SELECT u.*, p.* FROM "Users" u 
LEFT JOIN "Profiles" p ON u."Id" = p."UserId" 
WHERE u."Id" = @userId
```

### 2. **Role-Based Queries**
```sql
-- User roles
SELECT r.* FROM "Roles" r
JOIN "UserRoles" ur ON r."Id" = ur."RoleId"
WHERE ur."UserId" = @userId

-- Role claims
SELECT rc.* FROM "RoleClaims" rc
JOIN "UserRoles" ur ON rc."RoleId" = ur."RoleId"
WHERE ur."UserId" = @userId
```

### 3. **Address Management**
```sql
-- User shipping addresses
SELECT * FROM "ShippingAddresses" 
WHERE "UserId" = @userId 
ORDER BY "IsDefault" DESC, "UpdatedAt" DESC

-- Default address
SELECT * FROM "ShippingAddresses" 
WHERE "UserId" = @userId AND "IsDefault" = true
```

## Performance Optimization

### 1. **Indexing Strategy**
- **Primary Keys**: All tables have appropriate primary keys
- **Foreign Keys**: Indexed for join performance
- **Lookup Fields**: Email, username, and role name indexes
- **Composite Indexes**: UserRoles and UserTokens tables

### 2. **Query Optimization**
- **Normalized Fields**: Case-insensitive lookups
- **Selective Indexes**: Covering indexes for common queries
- **Join Optimization**: Proper foreign key relationships

### 3. **Data Integrity**
- **Foreign Key Constraints**: Referential integrity
- **Cascade Deletes**: Automatic cleanup of related data
- **Unique Constraints**: Prevent duplicate entries

## Migration Strategy

### 1. **Schema Evolution**
- **Backward Compatibility**: New fields are nullable
- **Default Values**: Sensible defaults for new columns
- **Migration Scripts**: Incremental schema updates

### 2. **Data Seeding**
```sql
-- Seed default roles
INSERT INTO "Roles" ("Id", "Name", "NormalizedName") VALUES
('admin', 'Administrator', 'ADMINISTRATOR'),
('user', 'User', 'USER'),
('moderator', 'Moderator', 'MODERATOR');

-- Seed admin user (password: Admin123!)
INSERT INTO "Users" ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash") VALUES
('admin-guid', 'admin', 'ADMIN', 'admin@example.com', 'ADMIN@EXAMPLE.COM', true, 'hashed-password');
```

## Best Practices

### 1. **Security Guidelines**
- **Password Hashing**: Use strong hashing algorithms
- **Token Security**: Secure storage of refresh tokens
- **Input Validation**: Validate all user inputs
- **SQL Injection Prevention**: Use parameterized queries

### 2. **Performance Guidelines**
- **Index Usage**: Monitor index performance
- **Query Optimization**: Use appropriate indexes
- **Connection Pooling**: Optimize database connections
- **Caching Strategy**: Cache frequently accessed user data

### 3. **Data Management**
- **Backup Strategy**: Regular database backups
- **Audit Trail**: Track user changes and access
- **Data Retention**: Implement data retention policies
- **Privacy Compliance**: GDPR and privacy regulation compliance

## Integration with Microservices

### 1. **API Gateway Integration**
- **JWT Tokens**: Token-based authentication
- **User Context**: Pass user information to downstream services
- **Role-Based Routing**: Route requests based on user roles

### 2. **Service Communication**
- **User Events**: Publish user creation/update events
- **Profile Events**: Share profile changes with other services
- **Address Events**: Notify ordering service of address changes

### 3. **Cross-Service Data**
- **User ID**: Consistent user identification across services
- **Profile Data**: Shared user profile information
- **Address Data**: Shipping address synchronization
