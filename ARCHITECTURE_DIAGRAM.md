# AddNewStaff Feature Architecture

## System Architecture Diagram

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
│  │  useAddNewStaff Hook                                     │  │
│  │  ┌────────────────────────────────────────────────────┐  │  │
│  │  │ • Validates form data                              │  │  │
│  │  │ • Calls addNewStaffAsync()                         │  │  │
│  │  │ • Shows toast notifications                        │  │  │
│  │  │ • Returns success/error status                     │  │  │
│  │  └────────────────────────────────────────────────────┘  │  │
│  │                          │                                │  │
│  │                          │ RTK Query mutation             │  │
│  │                          ▼                                │  │
│  │  identity.service.ts (RTK Query)                         │  │
│  │  ┌────────────────────────────────────────────────────┐  │  │
│  │  │ addNewStaff: builder.mutation<boolean, Request>    │  │  │
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

## Data Flow Sequence

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
    ├─► Form validation (client-side)
    │       ├─ Email format
    │       ├─ Password length
    │       ├─ Required fields
    │       └─ Phone number format
    │
    ├─► API Call (POST /api/v1/auth/add-new-staff)
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

## Component Interaction

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
│       └── useAddNewStaff Hook
│           ├── Form Validation
│           ├── API Call
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

## State Management

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

## Error Handling Flow

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



