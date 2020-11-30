# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

## copy csproj and restore as distinct layers
COPY ./DashboardApi/*.csproj /app
RUN dotnet restore

# copy everything else and build app
COPY ./DashboardApi .
#WORKDIR /app/build
RUN dotnet publish -c release -o ./build --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/build ./
ENTRYPOINT ["dotnet", "DashboardApi.dll"]