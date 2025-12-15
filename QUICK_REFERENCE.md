# AddNewStaff Feature - Quick Reference Guide

## 📋 Files Overview

### Backend Files

#### 1. AddNewStaffValidator.cs
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

#### 2. AddNewStaffRequestExample.cs
```csharp
Location: Services/Identity/YGZ.Identity.Api/Contracts/Auth/
Purpose: Swagger/OpenAPI documentation example
Usage: Automatically used by NSwag for API documentation
```

#### 3. AddNewStaffHandler.cs (Already Implemented)
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

#### 4. AddNewStaffCommand.cs (Already Implemented)
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

### Frontend Files

#### 1. identity.service.ts
```typescript
Location: apps/admin/src/infrastructure/services/identity.service.ts
Changes:
  - Added AddNewStaffRequest type
  - Added addNewStaff mutation
  - Exported useAddNewStaffMutation
Endpoint: POST /api/v1/auth/add-new-staff
```

#### 2. use-add-new-staff.ts
```typescript
Location: apps/admin/src/hooks/api/use-add-new-staff.ts
Purpose: Custom hook for AddNewStaff API calls
Exports:
  - addNewStaffAsync(data): Promise<Result>
  - isLoading: boolean
  - addNewStaffState: MutationState
Features:
  - Toast notifications
  - Error handling
  - Loading state
```

#### 3. add-new-staff-dialog.tsx
```typescript
Location: apps/admin/src/components/add-new-staff-dialog.tsx
Purpose: Reusable dialog component
Props:
  - onSuccess?: () => void
  - roles?: Array<{id: string; name: string}>
Features:
  - Form validation
  - Error messages
  - Loading state
  - Auto-fill tenant/branch from Redux
```

#### 4. hrm/page.tsx
```typescript
Location: apps/admin/src/app/dashboard/hrm/page.tsx
Changes:
  - Imported AddNewStaffDialog
  - Added dialog button in header
  - Implemented onSuccess callback
```

## 🔌 API Endpoint

### Request
```http
POST /api/v1/auth/add-new-staff
Authorization: Bearer {token}
Content-Type: application/json

{
  "email": "staff@example.com",
  "password": "SecurePassword123",
  "first_name": "John",
  "last_name": "Doe",
  "phone_number": "0987654321",
  "role_name": "STAFF",
  "tenant_id": "optional-id",
  "branch_id": "optional-id"
}
```

### Response (Success)
```http
200 OK
Content-Type: application/json

true
```

### Response (Error)
```http
400 Bad Request / 409 Conflict / 500 Internal Server Error
Content-Type: application/json

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

## 🧪 Testing Checklist

### Unit Tests
- [ ] AddNewStaffValidator validates all fields correctly
- [ ] AddNewStaffValidator rejects invalid email
- [ ] AddNewStaffValidator rejects short password
- [ ] AddNewStaffValidator rejects non-digit phone numbers

### Integration Tests
- [ ] Handler creates user in Keycloak
- [ ] Handler creates user in Database
- [ ] Handler assigns role correctly
- [ ] Handler rolls back on Keycloak failure
- [ ] Handler rolls back on Database failure
- [ ] Handler prevents duplicate email

### Frontend Tests
- [ ] Dialog opens on button click
- [ ] Form fields validate on submit
- [ ] Error messages display correctly
- [ ] Loading state shows during submission
- [ ] Success toast shows on success
- [ ] Error toast shows on failure
- [ ] Dialog closes on success
- [ ] User list refreshes after success
- [ ] Tenant/Branch auto-filled from Redux

### E2E Tests
- [ ] Add staff with valid data
- [ ] Add staff with invalid email
- [ ] Add staff with short password
- [ ] Add staff with duplicate email
- [ ] Add staff with invalid role
- [ ] Add staff with non-digit phone
- [ ] Verify staff appears in list
- [ ] Verify staff has correct role
- [ ] Verify staff has correct tenant/branch

## 🚀 Usage Example

### Using the Dialog in a Component
```typescript
import AddNewStaffDialog from '@components/add-new-staff-dialog';

export default function MyComponent() {
  const handleSuccess = () => {
    console.log('Staff added successfully');
    // Refresh data, etc.
  };

  return (
    <div>
      <AddNewStaffDialog
        onSuccess={handleSuccess}
        roles={[
          { id: 'STAFF', name: 'Staff' },
          { id: 'MANAGER', name: 'Manager' },
          { id: 'ADMIN', name: 'Admin' },
        ]}
      />
    </div>
  );
}
```

### Using the Hook Directly
```typescript
import useAddNewStaff from '~/src/hooks/api/use-add-new-staff';

export default function MyForm() {
  const { addNewStaffAsync, isLoading } = useAddNewStaff();

  const handleSubmit = async (formData) => {
    const result = await addNewStaffAsync(formData);
    if (result.isSuccess) {
      console.log('Staff added:', result.data);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {/* form fields */}
      <button disabled={isLoading}>
        {isLoading ? 'Adding...' : 'Add Staff'}
      </button>
    </form>
  );
}
```

## 🔍 Debugging Tips

### Frontend Debugging
1. Check Redux store for tenant/branch ID:
   ```typescript
   const { tenantId, branchId } = useAppSelector(state => state.tenant);
   console.log('Tenant:', tenantId, 'Branch:', branchId);
   ```

2. Check RTK Query cache:
   ```typescript
   // In Redux DevTools, look at identity-api state
   // Check addNewStaff mutation status
   ```

3. Check network request:
   ```typescript
   // In Browser DevTools > Network tab
   // Look for POST /api/v1/auth/add-new-staff
   // Check request body and response
   ```

### Backend Debugging
1. Check validator logs:
   ```csharp
   _logger.LogWarning("Validation failed for AddNewStaff: {Errors}", errors);
   ```

2. Check handler logs:
   ```csharp
   _logger.LogInformation("Creating staff user: {Email}", request.Email);
   _logger.LogError("Failed to create user: {Error}", ex.Message);
   ```

3. Check database:
   ```sql
   SELECT * FROM AspNetUsers WHERE Email = 'staff@example.com';
   SELECT * FROM AspNetUserRoles WHERE UserId = 'user-id';
   ```

4. Check Keycloak:
   - Admin console: Users > Search by email
   - Verify user attributes (tenant, branch)
   - Verify user roles

## 📝 Common Issues & Solutions

### Issue: "User already exists"
**Cause**: Email is already registered
**Solution**: Use a different email address

### Issue: "Role not found"
**Cause**: Role name doesn't exist in system
**Solution**: Check available roles in Keycloak admin console

### Issue: "Validation failed"
**Cause**: Invalid input data
**Solution**: Check error messages in response, fix validation errors

### Issue: "Failed to create user"
**Cause**: Database or Keycloak error
**Solution**: Check server logs for detailed error message

### Issue: Dialog doesn't close after success
**Cause**: onSuccess callback not implemented
**Solution**: Ensure onSuccess prop is passed to dialog

### Issue: User list doesn't refresh
**Cause**: RTK Query cache not invalidated
**Solution**: Check that addNewStaff mutation has `invalidatesTags: ['Users']`

## 🔐 Security Considerations

1. **Password Hashing**: Passwords are hashed using ASP.NET Identity
2. **Email Verification**: Staff accounts are pre-verified (emailConfirmed: true)
3. **Role-Based Access**: Only authenticated users can add staff
4. **Keycloak Integration**: User created in both Keycloak and local DB
5. **Rollback**: On failure, user is deleted from both systems
6. **Validation**: Both client and server-side validation

## 📚 Related Documentation

- [AddNewStaff Implementation Summary](./IMPLEMENTATION_SUMMARY.md)
- [Architecture Diagram](./ARCHITECTURE_DIAGRAM.md)
- [API Documentation](./Services/Identity/YGZ.Identity.Api/YGZ.Identity.Api.http)
- [Keycloak Integration](./Services/docs/keycloak-identity-server.md)

## 🆘 Support

For issues or questions:
1. Check the debugging tips above
2. Review the architecture diagram
3. Check server logs for detailed error messages
4. Check browser console for frontend errors
5. Review the test checklist for expected behavior







