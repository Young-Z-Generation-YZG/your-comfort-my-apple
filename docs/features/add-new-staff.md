# AddNewStaff Feature - Complete Documentation

## Table of Contents

1. [Overview](#overview)
2. [System Architecture](#system-architecture)
3. [Implementation Summary](#implementation-summary)
4. [Quick Reference Guide](#quick-reference-guide)
5. [API Documentation](#api-documentation)
6. [Testing](#testing)
7. [Debugging & Troubleshooting](#debugging--troubleshooting)

---

## Overview

The AddNewStaff feature allows administrators to create staff accounts for a tenant/branch. It includes both backend (.NET) and frontend (Next.js) components, integrating with Keycloak for identity management and a local SQL database.

### Key Features

✅ **Validation**: Both client-side and server-side validation  
✅ **Error Handling**: Comprehensive error messages and rollback on failure  
✅ **User Feedback**: Toast notifications for success/error states  
✅ **Loading States**: Disabled buttons during submission  
✅ **Auto-refresh**: User list updates after successful creation  
✅ **Tenant/Branch Support**: Automatically uses current tenant/branch from Redux  
✅ **Customizable Roles**: Roles can be passed as prop to dialog  
✅ **Keycloak Integration**: Creates user in both Keycloak and local database  
✅ **Password Hashing**: Passwords are securely hashed before storage

---

## System Architecture

### Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                     FRONTEND (@apps/admin)                       │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  HRM Page (dashboard/hrm/page.tsx)                              │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Header: "HRM - User Management"                          │  │
│  │ ┌────────────────────────────────────────────────────┐   │  │
│  │ │ [Add New Staff] Button                             │   │  │
│  │ └────────────────────────────────────────────────────┘   │  │
│  │                                                            │  │
│  │ User List Table (with pagination, filters)               │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          ▲                                       │
│                          │                                       │
│                    onSuccess callback                            │
│                    (refresh list)                                │
│                          │                                       │
│  ┌──────────────────────┴────────────────────────────────────┐  │
│  │                                                            │  │
│  │  AddNewStaffDialog Component                             │  │
│  │  ┌────────────────────────────────────────────────────┐  │  │
│  │  │ Form Fields:                                       │  │  │
│  │  │ • Email (required, email format)                   │  │  │
│  │  │ • Password (required, min 6 chars)                 │  │  │
│  │  │ • First Name (required)                            │  │  │
│  │  │ • Last Name (required)                             │  │  │
│  │  │ • Phone Number (required, digits only)             │  │  │
│  │  │ • Role (required, dropdown)                        │  │  │
│  │  │                                                    │  │  │
│  │  │ [Cancel] [Add Staff]                               │  │  │
│  │  └────────────────────────────────────────────────────┘  │  │
│  │                          │                                │  │
│  │                          │ form submission                │  │
│  │                          ▼                                │  │
│  │  Form Submission (react-hook-form)                      │  │
│  │  ┌────────────────────────────────────────────────────┐  │  │
│  │  │ • Validates with zod schema (defined inline)       │  │  │
│  │  │ • Form data: TAddNewStaffForm (Date objects)       │  │  │
│  │  │ • Transform to IAddNewStaffPayload                │  │  │
│  │  │   - birth_day.toISOString() (Date → string)       │  │  │
│  │  │   - Add tenant/branch from Redux                  │  │  │
│  │  └────────────────────────────────────────────────────┘  │  │
│  │                          │                                │  │
│  │                          │ payload: IAddNewStaffPayload   │  │
│  │                          ▼                                │  │
│  │  useAuthService Hook                                     │  │
│  │  ┌────────────────────────────────────────────────────┐  │  │
│  │  │ addNewStaffAsync(payload)                          │  │  │
│  │  │ • Wraps RTK Query mutation                         │  │  │
│  │  │ • Shows toast notifications                        │  │  │
│  │  │ • Returns { isSuccess, isError, data, error }     │  │  │
│  │  └────────────────────────────────────────────────────┘  │  │
│  │                          │                                │  │
│  │                          │ RTK Query mutation             │  │
│  │                          ▼                                │  │
│  │  identity.service.ts (RTK Query)                         │  │
│  │  ┌────────────────────────────────────────────────────┐  │  │
│  │  │ addNewStaff: builder.mutation<boolean, IAddNewStaffPayload>│  │
│  │  │ • Uses baseQuery('/identity-services')             │  │  │
│  │  │ • URL: /api/v1/auth/add-new-staff                  │  │  │
│  │  │ • Method: POST                                     │  │  │
│  │  │ • Invalidates: Users tag                           │  │  │
│  │  └────────────────────────────────────────────────────┘  │  │
│  └──────────────────────┬─────────────────────────────────────┘  │
│                         │                                        │
└─────────────────────────┼────────────────────────────────────────┘
                          │
                          │ HTTP POST
                          │ /api/v1/auth/add-new-staff
                          │
                          ▼
┌─────────────────────────────────────────────────────────────────┐
│                  API GATEWAY (YARP)                              │
│                  Routes to Identity Service                      │
└─────────────────────────────────────────────────────────────────┘
                          │
                          │
                          ▼
┌─────────────────────────────────────────────────────────────────┐
│              BACKEND (@Services/Identity)                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  AuthController.cs                                              │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ [HttpPost("add-new-staff")]                              │  │
│  │ public async Task<IActionResult> AddNewStaff(...)        │  │
│  │ {                                                         │  │
│  │   var cmd = _mapper.Map<AddNewStaffCommand>(request);    │  │
│  │   var result = await _sender.Send(cmd);                  │  │
│  │   return result.Match(onSuccess, onFailure);             │  │
│  │ }                                                         │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          │ MediatR Send                          │
│                          ▼                                       │
│  AddNewStaffCommand                                             │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Properties:                                              │  │
│  │ • Email                                                  │  │
│  │ • Password                                               │  │
│  │ • FirstName                                              │  │
│  │ • LastName                                               │  │
│  │ • PhoneNumber                                            │  │
│  │ • RoleName                                               │  │
│  │ • TenantId (optional)                                    │  │
│  │ • BranchId (optional)                                    │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          │ Validation                            │
│                          ▼                                       │
│  AddNewStaffValidator                                           │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Validates:                                               │  │
│  │ • Email format                                           │  │
│  │ • Password length (min 6)                                │  │
│  │ • Names not empty                                        │  │
│  │ • Phone number digits only                               │  │
│  │ • Role name not empty                                    │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          │ Valid ✓                               │
│                          ▼                                       │
│  AddNewStaffHandler                                             │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ 1. Check if user exists                                  │  │
│  │ 2. Validate role exists                                  │  │
│  │ 3. Create user in Keycloak                               │  │
│  │ 4. Create user in Database                               │  │
│  │ 5. Assign role to user                                   │  │
│  │ 6. Assign role in Keycloak                               │  │
│  │ 7. Return success/failure                                │  │
│  │                                                          │  │
│  │ Rollback on failure:                                     │  │
│  │ • Delete from Keycloak                                   │  │
│  │ • Delete from Database                                   │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          ▼                                       │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Keycloak (Identity Server)                               │  │
│  │ • Create user                                            │  │
│  │ • Set password                                           │  │
│  │ • Assign roles                                           │  │
│  │ • Set attributes (tenant, branch)                        │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ SQL Database (Identity DB)                               │  │
│  │ • Users table                                            │  │
│  │ • UserProfiles table                                     │  │
│  │ • UserRoles table                                        │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
└──────────────────────────┼───────────────────────────────────────┘
                           │
                           │ Response: true
                           │
                           ▼
┌─────────────────────────────────────────────────────────────────┐
│                  FRONTEND (@apps/admin)                          │
│                                                                   │
│  • Toast: "Staff member added successfully"                      │
│  • Dialog closes                                                 │
│  • User list refreshes (invalidates Users tag)                   │
│  • New staff appears in table                                    │
└─────────────────────────────────────────────────────────────────┘
```

### Data Flow Sequence

```
User Action
    │
    ├─► Click "Add New Staff" button
    │
    ├─► AddNewStaffDialog opens
    │
    ├─► Fill form fields
    │
    ├─► Click "Add Staff" button
    │
    ├─► Form validation (client-side, zod schema)
    │       ├─ Email format
    │       ├─ Password length (min 6)
    │       ├─ Required fields
    │       ├─ Phone number format (digits only)
    │       └─ Birth day (Date object)
    │
    ├─► Transform form data to payload
    │       ├─ birth_day.toISOString() (Date → string)
    │       └─ Add tenant/branch from Redux
    │
    ├─► API Call (POST /api/v1/auth/add-new-staff)
    │       └─ Headers auto-attached by baseQuery
    │
    ├─► Backend Processing
    │       ├─ Map request to command
    │       ├─ Validate command (server-side)
    │       ├─ Check user doesn't exist
    │       ├─ Create in Keycloak
    │       ├─ Create in Database
    │       ├─ Assign role
    │       └─ Return result
    │
    ├─► Response (true/error)
    │
    ├─► Frontend Handling
    │       ├─ Show toast notification
    │       ├─ Close dialog
    │       ├─ Refresh user list
    │       └─ Update table
    │
    └─► User sees new staff in list
```

### Component Interaction

```
HRM Page
├── Header Section
│   └── AddNewStaffDialog
│       ├── Dialog Trigger Button
│       ├── Dialog Content
│       │   ├── Form Fields
│       │   │   ├── Email Input
│       │   │   ├── Password Input
│       │   │   ├── FirstName Input
│       │   │   ├── LastName Input
│       │   │   ├── PhoneNumber Input
│       │   │   └── Role Select
│       │   └── Action Buttons
│       │       ├── Cancel Button
│       │       └── Add Staff Button
│       └── Form Submission Flow
│           ├── Zod Schema Validation (defined inline in component)
│           ├── Transform to Payload (IAddNewStaffPayload)
│           ├── useAuthService.addNewStaffAsync()
│           ├── identity.service.ts (RTK Query)
│           ├── Error Handling
│           └── Success Callback
│
├── Filter Section
│   ├── Search Inputs
│   └── Filter Dropdowns
│
└── Data Table
    ├── User List
    └── Pagination
```

### State Management

```
Redux Store
├── tenant
│   ├── tenantId
│   └── branchId
│
└── auth
    └── impersonatedUser

RTK Query Cache
├── Users (tag)
│   ├── getUsersByAdmin
│   ├── getUsers
│   └── getListUsers
│
└── addNewStaff (mutation)
    └── Invalidates: Users tag
```

### Error Handling Flow

```
Error Scenarios
│
├─► Validation Error (Client)
│   └─► Show field error messages
│
├─► Validation Error (Server)
│   └─► Toast: "Validation failed"
│
├─► User Already Exists
│   └─► Toast: "User with this email already exists"
│
├─► Role Not Found
│   └─► Toast: "Role does not exist"
│
├─► Keycloak Error
│   └─► Rollback database
│   └─► Toast: "Failed to create user"
│
├─► Database Error
│   └─► Rollback Keycloak
│   └─► Toast: "Failed to create user"
│
└─► Network Error
    └─► Toast: "Network error occurred"
```

---

## Implementation Summary

### Backend Implementation (@Services)

#### 1. **AddNewStaffValidator.cs**

-   **Location**: `Services/Identity/YGZ.Identity.Application/Auths/Commands/AddNewStaff/`
-   **Purpose**: Validates the AddNewStaffCommand before execution
-   **Validations**:
    -   Email: Required, must be valid email format
    -   Password: Required, minimum 6 characters
    -   FirstName: Required, max 100 characters
    -   LastName: Required, max 100 characters
    -   PhoneNumber: Required, digits only
    -   RoleName: Required

#### 2. **AddNewStaffRequestExample.cs**

-   **Location**: `Services/Identity/YGZ.Identity.Api/Contracts/Auth/`
-   **Purpose**: Provides Swagger/OpenAPI documentation example for the AddNewStaff endpoint
-   **Example Data**:
    ```json
    {
        "first_name": "John",
        "last_name": "Doe",
        "email": "john.doe@example.com",
        "password": "SecurePassword123",
        "phone_number": "0987654321",
        "role_name": "STAFF",
        "birth_day": "1990-01-01T00:00:00Z",
        "tenant_id": "664355f845e56534956be32b",
        "branch_id": "664357a235e84033bbd0e6b6"
    }
    ```

#### 3. **AddNewStaffHandler.cs** (Already Implemented)

-   Already implemented with comprehensive logic:
    -   Validates user doesn't already exist
    -   Creates user in Keycloak
    -   Creates user in database with hashed password
    -   Assigns requested role
    -   Handles rollback on failure

#### 4. **AddNewStaffCommand.cs** (Already Implemented)

-   Already defined with required properties:
    -   Email, Password, FirstName, LastName, PhoneNumber, RoleName, BirthDay
    -   Optional: TenantId, BranchId

### Frontend Implementation (@apps/admin)

#### Data Flow Architecture

Following the project's data flow pattern: **UI → Zod Schema → Payload → API Hook → Service**

1. **Zod Schema** (defined inline in `add-new-staff-dialog.tsx`)

    - Defines form validation rules directly in the component file
    - Uses `z.date()` for `birth_day` (Date object in form)
    - Defines `TAddNewStaffForm` type using `z.input<>`
    - Defines resolver: `addNewStaffResolver`
    - Supports conditional validation: `AddNewStaffSuperAdminSchema` extends base schema

2. **Payload Interface** (`~/src/domain/types/identity.type.ts`)

    - `IAddNewStaffPayload` interface (I prefix + Payload suffix)
    - Represents API contract (strings, not Date objects)
    - `birth_day` is string (ISO format)

3. **Service** (`~/src/infrastructure/services/identity.service.ts`)

    - Uses `baseQuery('/identity-services')` from base-query.ts
    - Defines `addNewStaff` mutation with typed request/response
    - Uses `IAddNewStaffPayload` from domain/types
    - Invalidates `Users` tag for cache consistency
    - Exports `useAddNewStaffMutation` hook

4. **API Hook** (`~/src/hooks/api/use-auth-service.ts`)

    - Wraps RTK Query mutation in `addNewStaffAsync` function
    - Returns normalized shape: `{ isSuccess, isError, data, error }`
    - Shows toast notifications using `toast.success()`
    - Handles errors gracefully

5. **UI Component** (`~/src/components/add-new-staff-dialog.tsx`)
    - Uses zod schema with `react-hook-form` via `zodResolver`
    - Form typed as `TAddNewStaffForm` (Date objects)
    - In `onSubmit`, transforms form data to payload:
        - `birth_day.toISOString()` (Date → string)
        - Adds tenant/branch from Redux context
    - Calls `addNewStaffAsync(payload)` with `IAddNewStaffPayload`

#### File Structure

**1. add-new-staff-dialog.tsx**

-   **Location**: `apps/admin/src/components/add-new-staff-dialog.tsx`
-   **Schema Definitions** (defined inline):
    -   `AddNewStaffSchema`: Base zod schema
    -   `AddNewStaffSuperAdminSchema`: Extended schema with required tenant/branch
    -   `TAddNewStaffForm`: Form type (`z.input<typeof AddNewStaffSchema>`)
    -   `addNewStaffResolver`: Resolver for react-hook-form
    -   `addNewStaffSuperAdminResolver`: Resolver for super admin form
-   **Component**: Reusable dialog component with form validation

**2. identity.type.ts**

-   **Location**: `apps/admin/src/domain/types/identity.type.ts`
-   **Exports**:
    -   `IAddNewStaffPayload`: API payload interface
    -   Properties: email, password, first_name, last_name, birth_day (string), phone_number, role_name, tenant_id, branch_id

**3. identity.service.ts**

-   **Location**: `apps/admin/src/infrastructure/services/identity.service.ts`
-   **Endpoint**: `POST /api/v1/auth/add-new-staff`
-   **Uses**: `baseQuery('/identity-services')` with automatic header handling
-   **Invalidates**: `Users` tag
-   **Exports**: `useAddNewStaffMutation` hook

**4. use-auth-service.ts**

-   **Location**: `apps/admin/src/hooks/api/use-auth-service.ts`
-   **Exports**: `addNewStaffAsync(payload: IAddNewStaffPayload)`
-   **Returns**: `{ isSuccess, isError, data, error }`
-   **Features**: Toast notifications, error handling

**5. add-new-staff-dialog.tsx**

-   **Location**: `apps/admin/src/components/add-new-staff-dialog.tsx`
-   **Schema**: Zod schemas defined inline at the top of the component file
-   **Props**:
    -   `onSuccess?: () => void`: Callback after successful creation
    -   `roles?: Array<{id: string; name: string}>`: Customizable roles (defaults to STAFF, ADMIN)
-   **Features**:
    -   Form validation with zod + react-hook-form
    -   Conditional schema based on user role (super admin vs regular admin)
    -   Auto-fills tenant/branch from Redux (`state.tenant`)
    -   Transforms Date to ISO string in onSubmit
    -   Loading state during submission
    -   Form reset on dialog close

**6. HRM Page Integration**

-   **Location**: `apps/admin/src/app/dashboard/hrm/page.tsx`
-   **Usage**: Imports `AddNewStaffDialog` component
-   **Integration**: Implements `onSuccess` callback to refresh user list

### User Experience Flow

1. Admin clicks "Add New Staff" button in HRM page header
2. Dialog opens with form fields
3. Admin fills in staff details
4. Form validates on submit
5. If valid, API call is made
6. On success:
    - Toast notification shows success message
    - Dialog closes
    - User list refreshes with new staff member
7. On error:
    - Toast notification shows error message
    - Dialog remains open for correction

### Files Modified/Created

#### Backend

-   ✅ Created: `Services/Identity/YGZ.Identity.Application/Auths/Commands/AddNewStaff/AddNewStaffValidator.cs`
-   ✅ Created: `Services/Identity/YGZ.Identity.Api/Contracts/Auth/AddNewStaffRequestExample.cs`

#### Frontend

-   ✅ Created: `apps/admin/src/components/add-new-staff-dialog.tsx` (Component with inline zod schemas)
-   ✅ Modified: `apps/admin/src/domain/types/identity.type.ts` (Added `IAddNewStaffPayload`)
-   ✅ Modified: `apps/admin/src/infrastructure/services/identity.service.ts` (Added mutation)
-   ✅ Modified: `apps/admin/src/hooks/api/use-auth-service.ts` (Added `addNewStaffAsync`)
-   ✅ Modified: `apps/admin/src/app/dashboard/hrm/page.tsx` (Integrated dialog)

### Key Implementation Principles

Following `apps/admin/.cursor/rules/project-rules-admin.mdc`:

1. **Separation of Concerns**:

    - **Zod Schemas** (defined inline in component files): UI form validation with Date objects and user-friendly types
    - **Payload Interfaces** (`~/src/domain/types/`): API contract with strings and backend-expected format
    - **Never pass zod schema types directly to API hooks/services**

2. **Data Flow Pattern**:

    ```
    UI Component → Zod Schema → Payload Interface → API Hook → Service
    ```

    - Transformation happens in UI layer (`onSubmit` handler)
    - Convert Date objects to ISO strings when creating payloads

3. **Type Safety**:

    - Form: `TAddNewStaffForm` (from `z.input<typeof AddNewStaffSchema>`)
    - Payload: `IAddNewStaffPayload` (from `~/src/domain/types`)
    - Response: `boolean` (from service)

4. **Service Layer**:

    - Uses `baseQuery('/identity-services')` from `base-query.ts`
    - Headers (`Authorization`, `X-TenantId`) auto-attached from Redux state
    - Never manually set headers

5. **API Hooks**:

    - Located in `~/src/hooks/api/use-auth-service.ts`
    - Return normalized shape: `{ isSuccess, isError, data, error }`
    - Handle toast notifications and side effects

6. **Naming Conventions**:
    - Domain types: `T` prefix (e.g., `TUser`, `TAddNewStaffForm`)
    - Payload interfaces: `I` prefix + `Payload` suffix (e.g., `IAddNewStaffPayload`)
    - Hooks: `use-` prefix + `-service` suffix when wrapping service

---

## Quick Reference Guide

### 📋 Files Overview

#### Backend Files

**1. AddNewStaffValidator.cs**

```csharp
Location: Services/Identity/YGZ.Identity.Application/Auths/Commands/AddNewStaff/
Purpose: Validates AddNewStaffCommand properties
Key Rules:
  - Email: Required, valid email format
  - Password: Required, min 6 characters
  - FirstName: Required, max 100 chars
  - LastName: Required, max 100 chars
  - PhoneNumber: Required, digits only
  - RoleName: Required
```

**2. AddNewStaffRequestExample.cs**

```csharp
Location: Services/Identity/YGZ.Identity.Api/Contracts/Auth/
Purpose: Swagger/OpenAPI documentation example
Usage: Automatically used by NSwag for API documentation
```

**3. AddNewStaffHandler.cs** (Already Implemented)

```csharp
Location: Services/Identity/YGZ.Identity.Application/Auths/Commands/AddNewStaff/
Purpose: Handles the business logic
Process:
  1. Validate user doesn't exist
  2. Create in Keycloak
  3. Create in Database
  4. Assign role
  5. Rollback on failure
```

**4. AddNewStaffCommand.cs** (Already Implemented)

```csharp
Location: Services/Identity/YGZ.Identity.Application/Auths/Commands/AddNewStaff/
Purpose: Command definition
Properties:
  - Email (required)
  - Password (required)
  - FirstName (required)
  - LastName (required)
  - PhoneNumber (required)
  - RoleName (required)
  - TenantId (optional)
  - BranchId (optional)
```

#### Frontend Files

**1. add-new-staff-dialog.tsx**

```typescript
Location: apps/admin/src/components/add-new-staff-dialog.tsx
Purpose: Component with inline zod schemas for form validation
Schema Definitions (at top of file):
  - AddNewStaffSchema: Base schema
  - AddNewStaffSuperAdminSchema: Extended schema with required tenant/branch
  - TAddNewStaffForm: Form type (z.input<typeof AddNewStaffSchema>)
  - addNewStaffResolver: Resolver for react-hook-form
  - addNewStaffSuperAdminResolver: Resolver for super admin
Key Schema Fields:
  - email: string (email format)
  - password: string (min 6 chars)
  - birth_day: Date (z.date())
  - first_name, last_name: string (required)
  - phone_number: string (digits only)
  - role_name: string (required)
  - tenant_id, branch_id: string (optional)
```

**2. identity.type.ts**

```typescript
Location: apps/admin/src/domain/types/identity.type.ts
Purpose: API payload interface definitions
Exports:
  - IAddNewStaffPayload: Payload interface (I prefix + Payload suffix)
Properties:
  - email, password, first_name, last_name: string
  - birth_day: string (ISO format)
  - phone_number, role_name: string
  - tenant_id, branch_id: string
```

**3. identity.service.ts**

```typescript
Location: apps/admin/src/infrastructure/services/identity.service.ts
Purpose: RTK Query service definition
Uses: baseQuery('/identity-services') from base-query.ts
Endpoint: POST /api/v1/auth/add-new-staff
Request Type: IAddNewStaffPayload (from domain/types)
Response Type: boolean
Cache: Invalidates 'Users' tag
Exports: useAddNewStaffMutation hook
```

**4. use-auth-service.ts**

```typescript
Location: apps/admin/src/hooks/api/use-auth-service.ts
Purpose: API hook wrapping RTK Query mutation
Exports:
  - addNewStaffAsync(payload: IAddNewStaffPayload): Promise<Result>
  - addNewStaffState: MutationState
Returns: { isSuccess, isError, data, error }
Features:
  - Toast notifications (toast.success)
  - Error handling
  - Normalized result shape
```

**5. add-new-staff-dialog.tsx**

```typescript
Location: apps/admin/src/components/add-new-staff-dialog.tsx
Purpose: Reusable dialog component with inline zod schemas
Schema: Zod schemas defined at the top of the component file
Props:
  - onSuccess?: () => void
  - roles?: Array<{id: string; name: string}>
Features:
  - Form validation with zod + react-hook-form
  - Conditional schema (super admin vs regular)
  - Auto-fill tenant/branch from Redux (state.tenant)
  - Transforms Date to ISO string in onSubmit
  - Loading state during submission
  - Form reset on dialog close
```

**6. hrm/page.tsx**

```typescript
Location: apps/admin/src/app/dashboard/hrm/page.tsx
Usage: Imports AddNewStaffDialog component
Integration: Implements onSuccess callback to refresh user list
```

### 🚀 Usage Examples

#### Using the Dialog in a Component

```typescript
import AddNewStaffDialog from "@components/add-new-staff-dialog";

export default function MyComponent() {
    const handleSuccess = () => {
        console.log("Staff added successfully");
        // Refresh data, invalidate queries, etc.
    };

    return (
        <div>
            <AddNewStaffDialog
                onSuccess={handleSuccess}
                roles={[
                    { id: "STAFF", name: "Staff" },
                    { id: "ADMIN", name: "Admin" },
                ]}
            />
        </div>
    );
}
```

**Note**: Default roles are `STAFF` and `ADMIN`. Customize via `roles` prop if needed.

#### Using the Hook Directly

```typescript
import useAuthService from "~/src/hooks/api/use-auth-service";
import { IAddNewStaffPayload } from "~/src/domain/types/identity.type";

export default function MyForm() {
    const { addNewStaffAsync, addNewStaffState } = useAuthService();

    const handleSubmit = async (formData: IAddNewStaffPayload) => {
        const result = await addNewStaffAsync(formData);
        if (result.isSuccess) {
            console.log("Staff added:", result.data);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            {/* form fields */}
            <button disabled={addNewStaffState.isLoading}>
                {addNewStaffState.isLoading ? "Adding..." : "Add Staff"}
            </button>
        </form>
    );
}
```

#### Data Flow Example (Form → API)

```typescript
// 1. Form uses zod schema (defined inline in component)
// Schema definitions at top of component file:
const AddNewStaffSchema = z.object({...});
type TAddNewStaffForm = z.input<typeof AddNewStaffSchema>;
const addNewStaffResolver = zodResolver(AddNewStaffSchema);

const form = useForm<TAddNewStaffForm>({
   resolver: addNewStaffResolver,
});

// 2. onSubmit transforms form data to payload
const onSubmit: SubmitHandler<TAddNewStaffForm> = async (data) => {
   const payload: IAddNewStaffPayload = {
      ...data,
      birth_day: data.birth_day.toISOString(), // Date → string
      tenant_id: tenantId || data.tenant_id || '',
      branch_id: branchId || data.branch_id || '',
   };

   // 3. Call API hook with payload
   const result = await addNewStaffAsync(payload);

   if (result.isSuccess) {
      // Handle success
   }
};
```

---

## API Documentation

### Endpoint

**POST** `/api/v1/auth/add-new-staff`

### Request

**Headers:**

```
Authorization: Bearer {token}
Content-Type: application/json
```

**Body:**

```json
{
    "email": "staff@example.com",
    "password": "SecurePassword123",
    "first_name": "John",
    "last_name": "Doe",
    "birth_day": "1990-01-01T00:00:00Z",
    "phone_number": "0987654321",
    "role_name": "STAFF",
    "tenant_id": "optional-id",
    "branch_id": "optional-id"
}
```

### Response (Success)

**Status:** `200 OK`

**Body:**

```json
true
```

### Response (Error)

**Status:** `400 Bad Request` / `409 Conflict` / `500 Internal Server Error`

**Body:**

```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "Email": ["Email is required"],
        "Password": ["Password must be at least 6 characters long"]
    }
}
```

### API Flow

**Request:**

```
POST /api/v1/auth/add-new-staff
Content-Type: application/json
Authorization: Bearer {token}
X-TenantId: {tenant-id}

{
  "email": "staff@example.com",
  "password": "SecurePassword123",
  "first_name": "John",
  "last_name": "Doe",
  "birth_day": "1990-01-01T00:00:00Z",
  "phone_number": "0987654321",
  "role_name": "STAFF",
  "tenant_id": "optional-tenant-id",
  "branch_id": "optional-branch-id"
}
```

**Note**: Headers (`Authorization`, `X-TenantId`) are automatically attached by `baseQuery` from Redux state. Do not set manually.

**Response:**

```
200 OK
true
```

---

## Testing

### Testing Recommendations

1. **Happy Path**: Add a new staff member with all valid data
2. **Validation**: Test each field validation (invalid email, short password, etc.)
3. **Duplicate Email**: Attempt to add staff with existing email
4. **Role Assignment**: Verify staff is assigned correct role
5. **Tenant/Branch**: Verify staff is assigned to correct tenant/branch
6. **List Refresh**: Verify new staff appears in user list after creation
7. **Error Handling**: Test with invalid role name or other API errors

### Testing Checklist

#### Unit Tests

-   [ ] AddNewStaffValidator validates all fields correctly
-   [ ] AddNewStaffValidator rejects invalid email
-   [ ] AddNewStaffValidator rejects short password
-   [ ] AddNewStaffValidator rejects non-digit phone numbers

#### Integration Tests

-   [ ] Handler creates user in Keycloak
-   [ ] Handler creates user in Database
-   [ ] Handler assigns role correctly
-   [ ] Handler rolls back on Keycloak failure
-   [ ] Handler rolls back on Database failure
-   [ ] Handler prevents duplicate email

#### Frontend Tests

-   [ ] Dialog opens on button click
-   [ ] Form fields validate on submit
-   [ ] Error messages display correctly
-   [ ] Loading state shows during submission
-   [ ] Success toast shows on success
-   [ ] Error toast shows on failure
-   [ ] Dialog closes on success
-   [ ] User list refreshes after success
-   [ ] Tenant/Branch auto-filled from Redux

#### E2E Tests

-   [ ] Add staff with valid data
-   [ ] Add staff with invalid email
-   [ ] Add staff with short password
-   [ ] Add staff with duplicate email
-   [ ] Add staff with invalid role
-   [ ] Add staff with non-digit phone
-   [ ] Verify staff appears in list
-   [ ] Verify staff has correct role
-   [ ] Verify staff has correct tenant/branch

---

## Debugging & Troubleshooting

### 🔍 Debugging Tips

#### Frontend Debugging

1. **Check Redux store for tenant/branch ID:**

    ```typescript
    import { useAppSelector } from "~/src/infrastructure/redux/store";

    const { tenantId, branchId } = useAppSelector((state) => state.tenant);
    console.log("Tenant:", tenantId, "Branch:", branchId);
    ```

2. **Check RTK Query cache:**

    ```typescript
    // In Redux DevTools, look at 'identity-api' reducer
    // Check mutations.addNewStaff status
    // Verify 'Users' tag invalidation
    ```

3. **Check network request:**

    ```typescript
    // In Browser DevTools > Network tab
    // Look for POST /api/v1/auth/add-new-staff
    // Verify headers: Authorization, X-TenantId (auto-attached by baseQuery)
    // Check request body matches IAddNewStaffPayload shape
    // Verify birth_day is ISO string format
    ```

4. **Check form data transformation:**

    ```typescript
    // In component onSubmit handler
    console.log("Form data (Date):", data.birth_day);
    console.log("Payload (ISO string):", payload.birth_day);
    ```

#### Backend Debugging

1. **Check validator logs:**

    ```csharp
    _logger.LogWarning("Validation failed for AddNewStaff: {Errors}", errors);
    ```

2. **Check handler logs:**

    ```csharp
    _logger.LogInformation("Creating staff user: {Email}", request.Email);
    _logger.LogError("Failed to create user: {Error}", ex.Message);
    ```

3. **Check database:**

    ```sql
    SELECT * FROM AspNetUsers WHERE Email = 'staff@example.com';
    SELECT * FROM AspNetUserRoles WHERE UserId = 'user-id';
    ```

4. **Check Keycloak:**
    - Admin console: Users > Search by email
    - Verify user attributes (tenant, branch)
    - Verify user roles

### 📝 Common Issues & Solutions

#### Issue: "User already exists"

**Cause**: Email is already registered  
**Solution**: Use a different email address

#### Issue: "Role not found"

**Cause**: Role name doesn't exist in system  
**Solution**: Check available roles in Keycloak admin console

#### Issue: "Validation failed"

**Cause**: Invalid input data  
**Solution**: Check error messages in response, fix validation errors

#### Issue: "Failed to create user"

**Cause**: Database or Keycloak error  
**Solution**: Check server logs for detailed error message

#### Issue: Dialog doesn't close after success

**Cause**: onSuccess callback not implemented  
**Solution**: Ensure onSuccess prop is passed to dialog

#### Issue: User list doesn't refresh

**Cause**: RTK Query cache not invalidated  
**Solution**: Verify `identity.service.ts` has `invalidatesTags: ['Users']` in `addNewStaff` mutation

#### Issue: birth_day format error

**Cause**: Date object passed instead of ISO string  
**Solution**: Ensure transformation in component: `birth_day: data.birth_day.toISOString()`

#### Issue: Headers not attached

**Cause**: Manual header setting or wrong baseQuery usage  
**Solution**: Use `baseQuery('/identity-services')` from base-query.ts; headers are auto-attached from Redux state

### 🔐 Security Considerations

1. **Password Hashing**: Passwords are hashed using ASP.NET Identity
2. **Email Verification**: Staff accounts are pre-verified (emailConfirmed: true)
3. **Role-Based Access**: Only authenticated users can add staff
4. **Keycloak Integration**: User created in both Keycloak and local DB
5. **Rollback**: On failure, user is deleted from both systems
6. **Validation**: Both client (zod) and server-side validation
7. **Header Management**: Authorization and X-TenantId headers managed by baseQuery from Redux state
8. **Type Safety**: Strong typing throughout (zod schemas → payload interfaces → API)

---

## Support

For issues or questions:

1. Check the debugging tips above
2. Review the architecture diagram
3. Check server logs for detailed error messages
4. Check browser console for frontend errors
5. Review the test checklist for expected behavior
