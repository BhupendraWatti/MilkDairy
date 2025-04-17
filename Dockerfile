# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all .csproj files
COPY ./MilkDairy/MilkDairy.csproj ./MilkDairy/MilkDairy.csproj
COPY ./MilkDairy.Model/MilkDairy.Model.csproj ./MilkDairy.Model/MilkDairy.Model.csproj
COPY ./MilkDairy.Utility/MilkDairy.Utility.csproj ./MilkDairy.Utility/MilkDairy.Utility.csproj
COPY ./MIlkDairyDataAccess/MIlkDairy.DataAccess.csproj ./MIlkDairyDataAccess/MIlkDairy.DataAccess.csproj

# Copy package.json and package-lock.json (if you have it)
COPY ./package.json ./package.json
COPY ./package-lock.json ./package-lock.json

# Copy the rest of the code
COPY . .

# Restore NuGet packages
RUN dotnet restore "MilkDairy/MilkDairy.csproj"

# Install Node.js and npm
RUN apt-get update && apt-get install -y nodejs npm

# Install Tailwind dependencies
RUN npm install

# Build Tailwind CSS (based on your script)
RUN npm run css:build

# Build and publish .NET project
RUN dotnet build "MilkDairy/MilkDairy.csproj" -c Release -o /app/build
RUN dotnet publish "MilkDairy/MilkDairy.csproj" -c Release -o /app/publish

# STAGE 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "MilkDairy.dll"]
