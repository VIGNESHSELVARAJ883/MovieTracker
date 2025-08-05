# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files first to restore dependencies
COPY ["MovieTracker.sln", "."]
COPY ["MovieTracker.API.csproj", "MovieTracker.API/"]
COPY ["MovieTracker.Service/MovieTracker.Service.csproj", "MovieTracker.Service/"]
COPY ["MovieTracker.Data/MovieTracker.Data.csproj", "MovieTracker.Data/"]


# Restore dependencies
WORKDIR /src/MovieTracker.API
RUN dotnet restore

# Copy entire source code
COPY . .

# Build the project
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieTracker.API.dll"]
