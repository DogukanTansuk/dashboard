# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Fetch and install Node 10. Make sure to include the --yes parameter 
# to automatically accept prompts during install, or it'll fail.
RUN curl --silent --location https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install --yes nodejs

## copy csproj and restore as distinct layers
COPY ./DashboardApi/*.csproj /app
RUN dotnet restore

# copy everything else and build app
COPY ./DashboardApi .
#WORKDIR /app/build
RUN dotnet publish -c release -o ./build --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY --from=build /app/build ./
ENTRYPOINT ["dotnet", "Dashboard.dll"]