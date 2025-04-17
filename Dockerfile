# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files
COPY ./MilkDairy/MilkDairy.csproj ./MilkDairy/
COPY ./MilkDairy.Model/MilkDairy.Model.csproj ./MilkDairy.Model/
COPY ./MilkDairy.Utility/MilkDairy.Utility.csproj ./MilkDairy.Utility/
COPY ./MIlkDairyDataAccess/MIlkDairy.DataAccess.csproj ./MIlkDairyDataAccess/

# Restore NuGet packages
RUN dotnet restore "MilkDairy/MilkDairy.csproj"

# Copy everything
COPY . .

# Install Node.js + npm
RUN apt-get update && apt-get install -y nodejs npm

# Install frontend dependencies and build Tailwind CSS
WORKDIR /src/MilkDairy
RUN npm install
RUN npm run css:build

# Build and publish .NET app
WORKDIR /src
RUN dotnet build "MilkDairy/MilkDairy.csproj" -c Release -o /app/build
RUN dotnet publish "MilkDairy/MilkDairy.csproj" -c Release -o /app/publish

# STAGE 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "MilkDairy.dll"]
