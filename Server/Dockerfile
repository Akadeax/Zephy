FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:3.1
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Server.dll"]