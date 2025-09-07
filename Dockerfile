# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY "demo api.csproj" ./
RUN dotnet restore

# Copy the rest of the application source code and build
COPY . .
RUN dotnet publish "demo api.csproj" -c Release -o /app/publish

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# The entry point for the container to run your application
ENTRYPOINT ["dotnet", "demo api.dll"]
