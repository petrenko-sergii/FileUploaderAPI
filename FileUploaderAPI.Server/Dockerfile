FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FileUploaderAPI.Server/FileUploaderAPI.Server.csproj", "FileUploaderAPI.Server/"]
COPY ["fileuploaderapi.client/fileuploaderapi.client.esproj", "fileuploaderapi.client/"]
RUN dotnet restore "./FileUploaderAPI.Server/FileUploaderAPI.Server.csproj"
COPY . .
WORKDIR "/src/FileUploaderAPI.Server"
RUN dotnet build "./FileUploaderAPI.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FileUploaderAPI.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileUploaderAPI.Server.dll"]