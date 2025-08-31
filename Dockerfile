# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/server/ReaderBuddy.WebApi.csproj", "src/server/"]
RUN dotnet restore "src/server/ReaderBuddy.WebApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/server"
RUN dotnet build "ReaderBuddy.WebApi.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ReaderBuddy.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReaderBuddy.WebApi.dll"]