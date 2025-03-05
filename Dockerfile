# Use .NET SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
EXPOSE 8080

# Copy everything in one step
COPY . ./

# Restore, build, and publish the .NET application in one step
WORKDIR /app/API
RUN dotnet restore && dotnet build && dotnet publish -c Release -o out

# Use a smaller runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/API/out .
ENTRYPOINT ["dotnet", "API.dll"]
