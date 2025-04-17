# Use the official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install Node.js and npm to handle CSS build
RUN apt-get update && apt-get install -y \
    curl \
    gnupg \
    ca-certificates \
    lsb-release \
    && curl -sL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y nodejs

# Copy the project file and restore dependencies
COPY ["MilkDairy/MilkDairy.csproj", "MilkDairy/"]
RUN dotnet restore "MilkDairy/MilkDairy.csproj"

# Copy the rest of the application files
COPY . .

# Set the working directory to the MilkDairy project folder
WORKDIR "/src/MilkDairy"

# Install npm dependencies before running css:build
RUN npm install

# Run the npm command to build CSS
RUN npm run css:build

# Build the project
RUN dotnet build "MilkDairy.csproj" -c Release -o /app/build

# Publish the project to the /app/publish folder
RUN dotnet publish "MilkDairy.csproj" -c Release -o /app/publish

# Final Stage - Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published files from the build image
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "MilkDairy.dll"]
