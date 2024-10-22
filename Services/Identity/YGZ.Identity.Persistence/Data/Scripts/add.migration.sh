#!/bin/bash

solutionName="YGZ.Identity"

dotnet ef \
    --project ../../../$solutionName.Persistence \
    --startup-project ../../../$solutionName.Api/ migrations add InitIdentityUser \
    --output-dir Data/Migrations/Identity \
    -c IdentityDbContext \

dotnet ef \
    --project ../../../$solutionName.Persistence \
    --startup-project ../../../$solutionName.Api/ migrations add InitialIdentityServer \
    --output-dir Data/Migrations/IdentityServer/PersistedGrantDb \
    -c PersistedGrantDbContext \

dotnet ef \
    --project ../../../$solutionName.Persistence \
    --startup-project ../../../$solutionName.Api/ migrations add InitialIdentityServerConfiguration \
    --output-dir Data/Migrations/IdentityServer/ConfigurationDb \
    -c ConfigurationDbContext \