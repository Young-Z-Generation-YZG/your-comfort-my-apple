# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Catalog/YGZ.Catalog.Api/YGZ.Catalog.Api.csproj", "Services/Catalog/YGZ.Catalog.Api/"]
COPY ["Services/Catalog/YGZ.Catalog.Application/YGZ.Catalog.Application.csproj", "Services/Catalog/YGZ.Catalog.Application/"]
COPY ["Services/BuildingBlocks/YGZ.BuildingBlocks.Shared/YGZ.BuildingBlocks.Shared.csproj", "Services/BuildingBlocks/YGZ.BuildingBlocks.Shared/"]
COPY ["Services/Catalog/YGZ.Catalog.Domain/YGZ.Catalog.Domain.csproj", "Services/Catalog/YGZ.Catalog.Domain/"]
COPY ["Services/Catalog/YGZ.Catalog.Infrastructure/YGZ.Catalog.Infrastructure.csproj", "Services/Catalog/YGZ.Catalog.Infrastructure/"]
COPY ["Services/BuildingBlocks/YGZ.BuildingBlocks.Messaging/YGZ.BuildingBlocks.Messaging.csproj", "Services/BuildingBlocks/YGZ.BuildingBlocks.Messaging/"]
RUN dotnet restore "./Services/Catalog/YGZ.Catalog.Api/YGZ.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/YGZ.Catalog.Api"
RUN dotnet build "./YGZ.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YGZ.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YGZ.Catalog.Api.dll"]