# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY src/Application/Application.csproj src/Application/
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/WebApi/WebApi.csproj src/WebApi/
COPY BlogApi.sln .
RUN dotnet restore

# Copy everything else
COPY . .

# Build the application
RUN dotnet publish src/WebApi/WebApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser

# Copy the published application
COPY --from=build /app/publish .

# Change ownership of the app folder to the appuser
RUN chown -R appuser:appuser .

# Switch to the non-root user
USER appuser

# Expose port
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "WebApi.dll"]