### PM> Add-Migration InitialCreate

### PM> Update-Database

### PM> Remove-Migration

## dotnet ef migrations add InitialCreate

## dotnet ef database update

if InitialCreate is your first migration, rollback to the baseline state by specifying 0:

# Update-Database 0

# Update-Database <new migrationName>

This will execute the Down method of InitialCreate, removing all changes applied by the migration.
