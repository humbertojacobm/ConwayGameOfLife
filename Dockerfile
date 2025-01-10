# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file
COPY ConwayGameOfLife.sln ./

# Copy all project files
COPY ConwayGameOfLife.API/*.csproj ConwayGameOfLife.API/
COPY ConwayGameOfLife.Core/*.csproj ConwayGameOfLife.Core/
COPY ConwayGameOfLife.Infrastructure/*.csproj ConwayGameOfLife.Infrastructure/
COPY ConwayGameOfLife.EntityLayer/*.csproj ConwayGameOfLife.EntityLayer/
COPY ConwayGameOfLife.DTO/*.csproj ConwayGameOfLife.DTO/
COPY ConwayGameOfLife.Common/*.csproj ConwayGameOfLife.Common/
COPY ConwayGameOfLife.Tests/*.csproj ConwayGameOfLife.Tests/

# Restore dependencies for the entire solution
RUN dotnet restore

# Copy the remaining source code for all projects
COPY ConwayGameOfLife.API/ ConwayGameOfLife.API/
COPY ConwayGameOfLife.Core/ ConwayGameOfLife.Core/
COPY ConwayGameOfLife.Infrastructure/ ConwayGameOfLife.Infrastructure/
COPY ConwayGameOfLife.EntityLayer/ ConwayGameOfLife.EntityLayer/
COPY ConwayGameOfLife.DTO/ ConwayGameOfLife.DTO/
COPY ConwayGameOfLife.Common/ ConwayGameOfLife.Common/
COPY ConwayGameOfLife.Tests/ ConwayGameOfLife.Tests/

# Publish the application
WORKDIR /src/ConwayGameOfLife.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "ConwayGameOfLife.API.dll"]