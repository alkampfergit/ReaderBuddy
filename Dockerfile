# Runtime stage - copy pre-built artifacts
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy pre-built WebAPI artifacts (from GitHub Actions build)
COPY publish/webapi .

ENTRYPOINT ["dotnet", "ReaderBuddy.WebApi.dll"]