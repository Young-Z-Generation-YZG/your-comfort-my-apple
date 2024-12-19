# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

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
COPY ["Services/Ordering/YGZ.Ordering.Api/YGZ.Ordering.Api.csproj", "Services/Ordering/YGZ.Ordering.Api/"]
COPY ["Services/BuildingBlocks/YGZ.BuildingBlocks.Messaging/YGZ.BuildingBlocks.Messaging.csproj", "Services/BuildingBlocks/YGZ.BuildingBlocks.Messaging/"]
COPY ["Services/Ordering/YGZ.Ordering.Application/YGZ.Ordering.Application.csproj", "Services/Ordering/YGZ.Ordering.Application/"]
COPY ["Services/BuildingBlocks/YGZ.BuildingBlocks.Shared/YGZ.BuildingBlocks.Shared.csproj", "Services/BuildingBlocks/YGZ.BuildingBlocks.Shared/"]
COPY ["Services/Ordering/YGZ.Ordering.Domain/YGZ.Ordering.Domain.csproj", "Services/Ordering/YGZ.Ordering.Domain/"]
COPY ["Services/Ordering/YGZ.Ordering.Contracts/YGZ.Ordering.Contracts.csproj", "Services/Ordering/YGZ.Ordering.Contracts/"]
COPY ["Services/Ordering/YGZ.Ordering.Infrastructure/YGZ.Ordering.Infrastructure.csproj", "Services/Ordering/YGZ.Ordering.Infrastructure/"]
COPY ["Services/Ordering/YGZ.Ordering.Persistence/YGZ.Ordering.Persistence.csproj", "Services/Ordering/YGZ.Ordering.Persistence/"]
RUN dotnet restore "./Services/Ordering/YGZ.Ordering.Api/YGZ.Ordering.Api.csproj"
COPY . .
WORKDIR "/src/Services/Ordering/YGZ.Ordering.Api"
RUN dotnet build "./YGZ.Ordering.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./YGZ.Ordering.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YGZ.Ordering.Api.dll"]