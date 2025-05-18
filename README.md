### PM> Add-Migration InitialCreate

### PM> Update-Database

### PM> Remove-Migration

## dotnet ef migrations add InitialCreate

## dotnet ef database update

if InitialCreate is your first migration, rollback to the baseline state by specifying 0:

# Update-Database 0

# Update-Database <new migrationName>

This will execute the Down method of InitialCreate, removing all changes applied by the migration.

# Ordering Service

PM>Add-Migration InitialCreate -OutputDir Data/Migrations -Project YGZ.Ordering.Persistence -StartupProject YGZ.Ordering.Api
dotnet cli>dotnet ef migrations add InitialCreate -o Data/Migrations -p YGZ.Ordering.Persistence -s YGZ.Ordering.Api

PM>Update-Database

# remove migration (Migrations folder)

dotnet ef migrations remove (remove latest migration'file which not updated yet)

# IdentityServer

dotnet ef migrations list

PS W:\projects\your-comfort-my-apple\Services\IdentityServer> dotnet ef --project YGZ.IdentityServer.Infrastructure --startup-project YGZ.IdentityServer.Api migrations add InitCreate --output-dir Persistence/Data/Migrations -c ApplicationDbContext

PS W:\projects\your-comfort-my-apple\Services\IdentityServer> dotnet ef --startup-project YGZ.IdentityServer.Api database update -c ApplicationDbContext

PS W:\projects\your-comfort-my-apple\Services\IdentityServer\YGZ.IdentityServer.Api> dotnet aspnet-codegenerator identity -dc YGZ.IdentityServer.Infrastructure.Persistence.Data.ApplicationDbContext

# Identity

PS W:\projects\your-comfort-my-apple\Services\Identity> dotnet ef migrations add Migration2 -o Persistence/Migrations -p YGZ.Identity.Infrastructure -s YGZ.Identity.Api

dotnet ef database update -p YGZ.Identity.Infrastructure -s YGZ.Identity.Api

### Docker 
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d

### export realm CLI
/opt/keycloak/bin/kc.sh export --dir /tmp/export  --realm ygz-realm   --users realm_file

### CP Docker to local
docker cp ygz.keycloak.server:/tmp/export ./DockerVolumes/KeycloakConfiguration/production