# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

USER app

WORKDIR /app
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/CleanArchitecture.Api/CleanArchitecture.Api.csproj", "src/CleanArchitecture.Api/"]
COPY ["src/CleanArchitecture.Infraestructure/CleanArchitecture.Infraestructure.csproj", "src/CleanArchitecture.Infraestructure/"]
COPY ["src/Core/CleanArchitecture.Application/CleanArchitecture.Application.csproj", "src/Core/CleanArchitecture.Application/"]
COPY ["src/Core/CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "src/Core/CleanArchitecture.Domain/"]
RUN dotnet restore "./src/CleanArchitecture.Api/CleanArchitecture.Api.csproj"
COPY . .
WORKDIR "/src/src/CleanArchitecture.Api"
RUN dotnet build "./CleanArchitecture.Api.csproj" -c Debug -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "./CleanArchitecture.Api.csproj" -c Debug -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Api.dll"]