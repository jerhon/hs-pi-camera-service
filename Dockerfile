
# Build the Application
FROM mcr.microsoft.com/dotnet/sdk:5.0 as build

# Copy project files and restore .NET dependencies
# Doing this first caches it so subsequent builds will be faster
COPY src/Honlsoft.Pi.CameraService/Honlsoft.Pi.CameraService.csproj /app/src/Honlsoft.Pi.CameraService/Honlsoft.Pi.CameraService.csproj
COPY src/Honlsoft.Pi.Hardware/Honlsoft.Pi.Hardware.csproj /app/src/Honlsoft.Pi.Hardware/Honlsoft.Pi.Hardware.csproj
COPY src/Honlsoft.Pi.CameraService.sln /app/src/Honlsoft.Pi.CameraService.sln
WORKDIR /app/src
RUN dotnet restore -r linux-arm

# Publish the application
COPY src /app/src
WORKDIR /app/src/Honlsoft.Pi.CameraService/
RUN dotnet publish -o /app/out

# Set up the final image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY --from=build /app/out /app
WORKDIR /app/out
ENTRYPOINT ["dotnet", "Honlsoft.Pi.CameraService"]