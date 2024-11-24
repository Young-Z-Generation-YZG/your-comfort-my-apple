# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Adjusted relative paths based on the build context
COPY ["Services/Identity/YGZ.Identity.Api/YGZ.Identity.Api.csproj", "Services/Identity/YGZ.Identity.Api/"]
COPY ["Services/Identity/YGZ.Identity.Application/YGZ.Identity.Application.csproj", "Services/Identity/YGZ.Identity.Application/"]
COPY ["Services/Identity/YGZ.Identity.Contracts/YGZ.Identity.Contracts.csproj", "Services/Identity/YGZ.Identity.Contracts/"]
COPY ["Services/Identity/YGZ.Identity.Domain/YGZ.Identity.Domain.csproj", "Services/Identity/YGZ.Identity.Domain/"]
COPY ["Services/Identity/YGZ.Identity.Infrastructure/YGZ.Identity.Infrastructure.csproj", "Services/Identity/YGZ.Identity.Infrastructure/"]
COPY ["Services/Identity/YGZ.Identity.Persistence/YGZ.Identity.Persistence.csproj", "Services/Identity/YGZ.Identity.Persistence/"]

RUN dotnet restore "Services/Identity/YGZ.Identity.Api/YGZ.Identity.Api.csproj"
COPY . .
WORKDIR "/src/Services/Identity/YGZ.Identity.Api"
RUN dotnet build "YGZ.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "YGZ.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YGZ.Identity.Api.dll"]
