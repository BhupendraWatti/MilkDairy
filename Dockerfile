# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory for the build stage
WORKDIR /src

# Copy the .csproj files for all the projects into the container (adjust the paths as per your actual structure)
COPY ./MilkDairy.csproj ./MilkDairy.csproj
COPY ./MilkDairy.Model/MilkDairy.Model.csproj ./MilkDairy.Model/MilkDairy.Model.csproj
COPY ./MilkDairy.Utility/MilkDairy.Utility.csproj ./MilkDairy.Utility/MilkDairy.Utility.csproj
COPY ./MIlkDairyDataAccess/MIlkDairy.DataAccess.csproj ./MIlkDairyDataAccess/MIlkDairy.DataAccess.csproj

# Copy the entire source code into the container
COPY ./ /src/

# Restore NuGet packages for the entire solution (including project references)
RUN dotnet restore "MilkDairy.csproj"

# Install Node.js and npm to run the npm command for building CSS (for Tailwind CSS)
RUN apt-get update && apt-get install -y nodejs npm

# Install project dependencies using npm
WORKDIR /src
RUN npm install

# Run the npm command to build CSS (for Tailwind)
RUN npm run css:build

# Build the MilkDairy project
RUN dotnet build "MilkDairy.csproj" -c Release -o /app/build

# Publish the application for production
RUN dotnet publish "MilkDairy.csproj" -c Release -o /app/publish

# Use the official .NET runtime image as the base image for the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory for the runtime stage
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose the port that the app will run on (adjust as needed)
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "MilkDairy.dll"]
