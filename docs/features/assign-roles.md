# Assign Roles Feature - Complete Documentation

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

The Assign Roles feature allows administrators to assign one or more roles to a user. This operation replaces the user's existing roles and syncs them to both Keycloak and ASP.NET Identity.

### Key Features

✅ **Role Management**: Assign multiple roles to a user  
✅ **Role Replacement**: Replaces existing roles (not append)  
✅ **Keycloak Integration**: Syncs roles to Keycloak and local database  
✅ **Error Handling**: Comprehensive error messages and validation  
✅ **User Feedback**: Toast notifications for success/error states  
✅ **Auto-refresh**: User data updates after successful assignment  
✅ **Multi-Tenant Support**: Automatically uses current tenant from Redux  
✅ **Type Safety**: Strong typing throughout the data flow

---

## System Architecture

### Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                     FRONTEND (@apps/admin)                       │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  HRM Staff Detail Page (dashboard/hrm/[id]/page.tsx)            │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Roles and Permissions Section                            │  │
│  │ ┌────────────────────────────────────────────────────┐   │  │
│  │ │ Multi-select Role Dropdown                          │   │  │
│  │ │ • ADMIN                                             │   │  │
│  │ │ • ADMIN_SUPER                                       │   │  │
│  │ │ • STAFF                                             │   │  │
│  │ │ • USER                                              │   │  │
│  │ │                                                      │   │  │
│  │ │ [Assign Roles] Button                               │   │  │
│  │ └────────────────────────────────────────────────────┘   │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          │ form submission                       │
│                          ▼                                       │
│  Component onSubmit Handler                                     │  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ • Validates selected roles                               │  │
│  │ • Creates IAssignRolesPayload                            │  │
│  │ • Calls assignRolesAsync(payload)                        │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          │ payload: IAssignRolesPayload         │
│                          ▼                                       │
│  useUserService Hook                                            │  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ assignRolesAsync(payload)                                 │  │
│  │ • Wraps RTK Query mutation                               │  │
│  │ • Shows toast notifications                              │  │
│  │ • Returns { isSuccess, isError, data, error }           │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          │ RTK Query mutation                    │
│                          ▼                                       │
│  user.service.ts (RTK Query)                                    │  │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ assignRoles: builder.mutation<boolean, IAssignRolesPayload>│  │
│  │ • Uses baseQuery('/identity-services')                  │  │
│  │ • URL: /api/v1/users/{user_id}/roles                    │  │
│  │ • Method: PUT                                           │  │
│  │ • Body: { roles: string[] }                            │  │
│  │ • Invalidates: Users, UserRoles tags                   │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
└──────────────────────────┼───────────────────────────────────────┘
                          │
                          │ HTTP PUT
                          │ /api/v1/users/{user_id}/roles
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
│  UserController.cs                                               │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ [HttpPut("{userId}/roles")]                               │  │
│  │ public async Task<IActionResult> AssignRoles(...)        │  │
│  │ {                                                         │  │
│  │   var cmd = _mapper.Map<AssignRolesCommand>(request);    │  │
│  │   var result = await _sender.Send(cmd);                  │  │
│  │   return result.Match(onSuccess, onFailure);             │  │
│  │ }                                                         │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          │ MediatR Send                          │
│                          ▼                                       │
│  AssignRolesCommand                                             │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Properties:                                              │  │
│  │ • UserId (from path)                                    │  │
│  │ • Roles (from body)                                      │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          │ Validation                            │
│                          ▼                                       │
│  AssignRolesHandler                                             │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ 1. Validate user exists                                  │  │
│  │ 2. Validate roles exist in Keycloak                      │  │
│  │ 3. Remove existing roles from ASP.NET Identity           │  │
│  │ 4. Add new roles to ASP.NET Identity                    │  │
│  │ 5. Sync roles to Keycloak                                │  │
│  │ 6. Return success/failure                                │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│                          ▼                                       │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ Keycloak (Identity Server)                               │  │
│  │ • Assign roles to user                                   │  │
│  │ • Remove old roles                                       │  │
│  └──────────────────────────────────────────────────────────┘  │
│                          │                                       │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ SQL Database (Identity DB)                               │  │
│  │ • UserRoles table                                        │  │
│  │ • Update role assignments                                │  │
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
│  • Toast: "Roles assigned successfully"                         │
│  • User data refreshes (invalidates Users, UserRoles tags)      │
│  • Updated roles appear in UI                                    │
└─────────────────────────────────────────────────────────────────┘
```

### Data Flow Sequence

```
User Action
    │
    ├─► Open staff detail page
    │
    ├─► Select roles from multi-select dropdown
    │
    ├─► Click "Assign Roles" button
    │
    ├─► Client-side validation
    │       ├─ user_id must be non-empty string
    │       └─ roles must be non-empty array
    │
    ├─► Create IAssignRolesPayload
    │       ├─ user_id: string (from route params)
    │       └─ roles: string[] (from form selection)
    │
    ├─► API Call (PUT /api/v1/users/{user_id}/roles)
    │       └─ Headers auto-attached by baseQuery
    │
    ├─► Backend Processing
    │       ├─ Validate user exists
    │       ├─ Validate roles exist in Keycloak
    │       ├─ Remove existing roles from ASP.NET Identity
    │       ├─ Add new roles to ASP.NET Identity
    │       ├─ Sync roles to Keycloak
    │       └─ Return result
    │
    ├─► Response (true/error)
    │
    ├─► Frontend Handling
    │       ├─ Show toast notification
    │       ├─ Invalidate Users and UserRoles tags
    │       ├─ Refresh user data
    │       └─ Update UI
    │
    └─► User sees updated roles
```

### Component Interaction

```
HRM Staff Detail Page
├── User Information Section
│   └── User details display
│
├── Roles and Permissions Section
│   ├── Current Roles Display
│   ├── Role Multi-Select Dropdown
│   │   ├── ADMIN option
│   │   ├── ADMIN_SUPER option
│   │   ├── STAFF option
│   │   └── USER option
│   └── Assign Roles Button
│       └── onSubmit Handler
│           ├── Create Payload
│           ├── Call assignRolesAsync
│           ├── Error Handling
│           └── Success Callback
│
└── Other User Details
```

### State Management

```
Redux Store
├── tenant
│   └── tenantId (auto-attached to requests)
│
└── auth
    └── currentUser (for permission checks)

RTK Query Cache
├── Users (tag)
│   ├── getUserByUserId
│   └── getAccountDetails
│
├── UserRoles (tag)
│   └── getUserRoles
│
└── assignRoles (mutation)
    └── Invalidates: Users, UserRoles tags
```

### Error Handling Flow

```
Error Scenarios
│
├─► Validation Error (Client)
│   └─► Show field error messages
│
├─► User Not Found (404)
│   └─► Toast: "User not found"
│
├─► Role Not Found (404)
│   └─► Toast: "Role does not exist"
│
├─► Keycloak Error
│   └─► Toast: "Failed to assign roles"
│
├─► Validation Error (Server)
│   └─► Toast: "Validation failed"
│
└─► Network Error
    └─► Toast: "Network error occurred"
```

---

## Implementation Summary

### Backend Implementation (@Services)

#### 1. **AssignRolesHandler.cs** (Already Implemented)

- Already implemented with comprehensive logic:
   - Validates user exists
   - Validates roles exist in Keycloak
   - Removes existing roles from ASP.NET Identity
   - Adds new roles to ASP.NET Identity
   - Syncs roles to Keycloak
   - Returns success/failure

#### 2. **AssignRolesCommand.cs** (Already Implemented)

- Already defined with required properties:
   - UserId: string (from path parameter)
   - Roles: string[] (from request body)

### Frontend Implementation (@apps/admin)

#### Data Flow Architecture

Following the project's data flow pattern: **UI → Payload → API Hook → Service**

**Note**: Unlike AddNewStaff, this feature does not require a zod schema since it's a simple role selection without complex form validation. The payload is created directly from UI selections.

1. **Payload Interface** (`~/src/domain/types/identity.type.ts`)

   - `IAssignRolesPayload` interface (I prefix + Payload suffix)
   - Represents API contract
   - Properties:
      - `user_id`: string (Identity user id, passed via path)
      - `roles`: string[] (role names, sent in body)

2. **Service** (`~/src/infrastructure/services/user.service.ts`)

   - Uses `baseQuery('/identity-services')` from base-query.ts
   - Defines `assignRoles` mutation with typed request/response
   - Uses `IAssignRolesPayload` from domain/types
   - URL pattern: `/api/v1/users/${data.user_id}/roles`
   - Method: PUT
   - Body: `{ roles: data.roles }` (user_id extracted from payload for path)
   - Invalidates `Users` and `UserRoles` tags for cache consistency
   - Exports `useAssignRolesMutation` hook

3. **API Hook** (`~/src/hooks/api/use-user-service.ts`)

   - Wraps RTK Query mutation in `assignRolesAsync` function
   - Returns normalized shape: `{ isSuccess, isError, data, error }`
   - Shows toast notifications using `toast.success()`
   - Handles errors gracefully

4. **UI Component** (Planned - owned by another agent)

   - Multi-select dropdown for roles
   - Gets `user_id` from route params (`[id]`)
   - Creates `IAssignRolesPayload` from selections
   - Calls `assignRolesAsync(payload)`

#### File Structure

**1. identity.type.ts**

- **Location**: `apps/admin/src/domain/types/identity.type.ts`
- **Exports**:
   - `IAssignRolesPayload`: API payload interface
   - Properties:
      - `user_id`: string (Identity user id)
      - `roles`: string[] (role names)

**2. user.service.ts**

- **Location**: `apps/admin/src/infrastructure/services/user.service.ts`
- **Endpoint**: `PUT /api/v1/users/{user_id}/roles`
- **Uses**: `baseQuery('/identity-services')` with automatic header handling
- **Request**: `IAssignRolesPayload` (user_id used in path, roles in body)
- **Response**: `boolean`
- **Invalidates**: `Users`, `UserRoles` tags
- **Exports**: `useAssignRolesMutation` hook

**3. use-user-service.ts**

- **Location**: `apps/admin/src/hooks/api/use-user-service.ts`
- **Exports**: `assignRolesAsync(payload: IAssignRolesPayload)`
- **Returns**: `{ isSuccess, isError, data, error }`
- **Features**: Toast notifications, error handling

**4. role.enum.ts**

- **Location**: `apps/admin/src/domain/enums/role.enum.ts`
- **Exports**: `ERole` enum
- **Values**: `ADMIN`, `ADMIN_SUPER`, `STAFF`, `USER`

### User Experience Flow

1. Admin navigates to staff detail page (`/dashboard/hrm/[id]`)
2. Admin views current roles assigned to user
3. Admin selects one or more roles from multi-select dropdown
4. Admin clicks "Assign Roles" button
5. If valid, API call is made
6. On success:
   - Toast notification shows success message
   - User data refreshes with new roles
   - UI updates to show assigned roles
7. On error:
   - Toast notification shows error message
   - UI remains interactive for retry

### Files Modified/Created

#### Backend

- ✅ Already Implemented: `Services/Identity/YGZ.Identity.Application/Users/Commands/AssignRoles/`

#### Frontend

- ✅ Modified: `apps/admin/src/domain/types/identity.type.ts` (Added `IAssignRolesPayload`)
- ✅ Modified: `apps/admin/src/infrastructure/services/user.service.ts` (Added mutation)
- ✅ Modified: `apps/admin/src/hooks/api/use-user-service.ts` (Added `assignRolesAsync`)
- ⏳ Planned: `apps/admin/src/app/dashboard/hrm/[id]/page.tsx` (UI implementation - owned by another agent)

---

## Quick Reference Guide

### 📋 Files Overview

#### Frontend Files

**1. identity.type.ts**

```typescript
Location: apps/admin/src/domain/types/identity.type.ts
Purpose: API payload interface definition
Exports:
  - IAssignRolesPayload: Payload interface (I prefix + Payload suffix)
Properties:
  - user_id: string (Identity user id, used in URL path)
  - roles: string[] (role names, sent in request body)
```

**2. user.service.ts**

```typescript
Location: apps/admin/src/infrastructure/services/user.service.ts
Purpose: RTK Query service definition
Uses: baseQuery('/identity-services') from base-query.ts
Endpoint: PUT /api/v1/users/{user_id}/roles
Request Type: IAssignRolesPayload (from domain/types)
Response Type: boolean
Cache: Invalidates 'Users', 'UserRoles' tags
Exports: useAssignRolesMutation hook
Note: user_id extracted from payload for URL path, roles sent in body
```

**3. use-user-service.ts**

```typescript
Location: apps/admin/src/hooks/api/use-user-service.ts
Purpose: API hook wrapping RTK Query mutation
Exports:
  - assignRolesAsync(payload: IAssignRolesPayload): Promise<Result>
Returns: { isSuccess, isError, data, error }
Features:
  - Toast notifications (toast.success)
  - Error handling
  - Normalized result shape
```

**4. role.enum.ts**

```typescript
Location: apps/admin/src/domain/enums/role.enum.ts
Purpose: Centralized role values
Exports:
  - ERole enum
Values:
  - ADMIN
  - ADMIN_SUPER
  - STAFF
  - USER
```

### 🚀 Usage Examples

#### Using the Hook in a Component

```typescript
import useUserService from '~/src/hooks/api/use-user-service';
import { IAssignRolesPayload } from '~/src/domain/types/identity.type';
import { ERole } from '~/src/domain/enums/role.enum';
import { useParams } from 'next/navigation';

export default function StaffDetailPage() {
  const params = useParams();
  const userId = params.id as string;
  const { assignRolesAsync, assignRolesState } = useUserService();
  const [selectedRoles, setSelectedRoles] = useState<string[]>([]);

  const handleAssignRoles = async () => {
    const payload: IAssignRolesPayload = {
      user_id: userId,
      roles: selectedRoles,
    };

    const result = await assignRolesAsync(payload);

    if (result.isSuccess) {
      console.log('Roles assigned successfully');
      // Refresh user data, etc.
    }
  };

  return (
    <div>
      {/* Role selection UI */}
      <button
        onClick={handleAssignRoles}
        disabled={assignRolesState.isLoading}
      >
        {assignRolesState.isLoading ? 'Assigning...' : 'Assign Roles'}
      </button>
    </div>
  );
}
```

#### Data Flow Example (UI → API)

```typescript
// 1. Get user_id from route params
const params = useParams();
const userId = params.id as string;

// 2. Get selected roles from UI (multi-select dropdown)
const selectedRoles = ['ADMIN', 'STAFF']; // From form state

// 3. Create payload
const payload: IAssignRolesPayload = {
   user_id: userId,
   roles: selectedRoles,
};

// 4. Call API hook with payload
const result = await assignRolesAsync(payload);

if (result.isSuccess) {
   // Handle success - user data will auto-refresh via cache invalidation
}
```

### Key Implementation Principles

Following `apps/admin/.cursor/rules/project-rules-admin.mdc`:

1. **Payload Interface Pattern**:

   - **Payload Interfaces** (`~/src/domain/types/`): API contract with backend-expected format
   - `user_id` passed via URL path (extracted from payload in service)
   - `roles` sent in request body

2. **Data Flow Pattern**:

   ```
   UI Component → Payload Interface → API Hook → Service
   ```

   - No zod schema needed (simple role selection)
   - Payload created directly from UI selections

3. **Type Safety**:

   - Payload: `IAssignRolesPayload` (from `~/src/domain/types`)
   - Roles: Use `ERole` enum values to avoid typos
   - Response: `boolean` (from service)

4. **Service Layer**:

   - Uses `baseQuery('/identity-services')` from `base-query.ts`
   - Headers (`Authorization`, `X-TenantId`) auto-attached from Redux state
   - Never manually set headers
   - URL path includes `user_id`: `/api/v1/users/${data.user_id}/roles`
   - Body contains only `roles`: `{ roles: data.roles }`

5. **API Hooks**:

   - Located in `~/src/hooks/api/use-user-service.ts`
   - Return normalized shape: `{ isSuccess, isError, data, error }`
   - Handle toast notifications and side effects

6. **Naming Conventions**:

   - Payload interfaces: `I` prefix + `Payload` suffix (e.g., `IAssignRolesPayload`)
   - Hooks: `use-` prefix + `-service` suffix when wrapping service
   - Async functions: `assignRolesAsync` (ends with `Async`)

7. **Cache Management**:

   - Invalidates both `Users` and `UserRoles` tags
   - Ensures user data and role assignments refresh after mutation

---

## API Documentation

### Endpoint

**PUT** `/api/v1/users/{user_id}/roles`

### Request

**Headers:**

```
Authorization: Bearer {token}
Content-Type: application/json
X-TenantId: {tenant-id}
```

**Path Parameters:**

- `user_id` (string, required): Identity user id (GUID format)

**Body:**

```json
{
   "roles": ["ADMIN", "STAFF"]
}
```

### Response (Success)

**Status:** `200 OK`

**Body:**

```json
true
```

### Response (Error)

**Status:** `400 Bad Request` / `404 Not Found` / `500 Internal Server Error`

**Body:**

```json
{
   "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
   "title": "One or more validation errors occurred.",
   "status": 400,
   "errors": {
      "Roles": ["Roles array cannot be empty"],
      "UserId": ["User does not exist"]
   }
}
```

### API Flow

**Request:**

```
PUT /api/v1/users/be0cd669-237a-484d-b3cf-793e0ad1b0ea/roles
Content-Type: application/json
Authorization: Bearer {token}
X-TenantId: {tenant-id}

{
  "roles": ["ADMIN", "STAFF"]
}
```

**Response:**

```
200 OK
true
```

**Note**: Headers (`Authorization`, `X-TenantId`) are automatically attached by `baseQuery` from Redux state. Do not set manually.

### Allowed Role Values

Roles must be one of the following (from `ERole` enum):

- `ADMIN`
- `ADMIN_SUPER`
- `STAFF`
- `USER`

---

## Testing

### Testing Recommendations

1. **Happy Path**: Assign single role to user
2. **Multiple Roles**: Assign multiple roles to user
3. **Role Replacement**: Verify existing roles are replaced (not appended)
4. **Invalid Role**: Attempt to assign non-existent role
5. **Invalid User**: Attempt to assign roles to non-existent user
6. **Empty Roles**: Attempt to assign empty roles array
7. **Cache Refresh**: Verify user data refreshes after assignment
8. **Keycloak Sync**: Verify roles sync to Keycloak correctly

### Testing Checklist

#### Unit Tests

- [ ] assignRolesAsync returns success for valid payload
- [ ] assignRolesAsync returns error for invalid user_id
- [ ] assignRolesAsync returns error for empty roles array
- [ ] assignRolesAsync returns error for invalid role names

#### Integration Tests

- [ ] Handler assigns roles to user in database
- [ ] Handler syncs roles to Keycloak
- [ ] Handler replaces existing roles (not append)
- [ ] Handler validates user exists
- [ ] Handler validates roles exist in Keycloak

#### Frontend Tests

- [ ] Hook shows success toast on success
- [ ] Hook shows error toast on failure
- [ ] Hook returns correct result shape
- [ ] Service invalidates Users tag
- [ ] Service invalidates UserRoles tag
- [ ] User data refreshes after assignment

#### E2E Tests

- [ ] Assign single role -> success
- [ ] Assign multiple roles -> success
- [ ] Assign invalid role name -> error toast
- [ ] Assign to non-existent user -> error toast
- [ ] Verify roles are replaced (not appended) on subsequent fetch
- [ ] Verify Keycloak assignments match Identity roles

---

## Debugging & Troubleshooting

### 🔍 Debugging Tips

#### Frontend Debugging

1. **Check user_id from route params:**

   ```typescript
   import { useParams } from 'next/navigation';

   const params = useParams();
   const userId = params.id as string;
   console.log('User ID:', userId);
   ```

2. **Check selected roles:**

   ```typescript
   console.log('Selected roles:', selectedRoles);
   console.log('Payload:', payload);
   ```

3. **Check RTK Query cache:**

   ```typescript
   // In Redux DevTools, look at 'user-api' reducer
   // Check mutations.assignRoles status
   // Verify 'Users' and 'UserRoles' tags invalidation
   ```

4. **Check network request:**

   ```typescript
   // In Browser DevTools > Network tab
   // Look for PUT /api/v1/users/{user_id}/roles
   // Verify headers: Authorization, X-TenantId (auto-attached by baseQuery)
   // Check request body matches { roles: string[] }
   // Verify user_id is in URL path
   ```

#### Backend Debugging

1. **Check handler logs:**

   ```csharp
   _logger.LogInformation("Assigning roles to user: {UserId}", request.UserId);
   _logger.LogError("Failed to assign roles: {Error}", ex.Message);
   ```

2. **Check database:**

   ```sql
   SELECT * FROM AspNetUserRoles WHERE UserId = 'user-id';
   ```

3. **Check Keycloak:**
   - Admin console: Users > Search by user id
   - Verify role assignments
   - Verify roles match database

### 📝 Common Issues & Solutions

#### Issue: "User not found"

**Cause**: Invalid or non-existent user_id  
**Solution**: Verify user_id is correct and user exists in database

#### Issue: "Role not found"

**Cause**: Role name doesn't exist in Keycloak  
**Solution**: Check available roles in Keycloak admin console, use `ERole` enum values

#### Issue: "Roles array cannot be empty"

**Cause**: Empty roles array sent in request  
**Solution**: Ensure at least one role is selected before submitting

#### Issue: Roles not updating in UI

**Cause**: Cache not invalidated  
**Solution**: Verify `assignRoles` mutation has `invalidatesTags: ['Users', 'UserRoles']`

#### Issue: Roles appended instead of replaced

**Cause**: Backend logic issue  
**Solution**: Verify backend handler removes existing roles before adding new ones

#### Issue: Headers not attached

**Cause**: Manual header setting or wrong baseQuery usage  
**Solution**: Use `baseQuery('/identity-services')` from base-query.ts; headers are auto-attached from Redux state

### 🔐 Security Considerations

1. **Role-Based Access**: Only admin-level users should assign roles
2. **Multi-Tenant**: Tenant header (`X-TenantId`) managed by baseQuery from Redux state
3. **Validation**: Both client and server-side validation
4. **Keycloak Integration**: Roles synced to both Keycloak and local database
5. **Role Replacement**: Existing roles are replaced (not appended) for security
6. **Type Safety**: Use `ERole` enum to prevent invalid role assignments

---

## Support

For issues or questions:

1. Check the debugging tips above
2. Review the architecture diagram
3. Check server logs for detailed error messages
4. Check browser console for frontend errors
5. Review the test checklist for expected behavior
6. Verify role values match `ERole` enum
7. Ensure `user_id` is correctly extracted from route params
