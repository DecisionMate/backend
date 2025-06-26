FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DecisionMate.slnx", "/"]
COPY ["nuget.config", "/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/Integrations/Integrations.csproj", "Integrations/"]
COPY ["src/Web/Web.csproj", "Web/"]
COPY ["src/Directory.Build.props", "/"]
COPY ["src/Directory.Packages.props", "/"]
RUN --mount=type=secret,id=github-username \
    --mount=type=secret,id=github-token \
    dotnet nuget update source Github \
                    --username $(cat /run/secrets/github-username) \
                    --password $(cat /run/secrets/github-token) \
                    --store-password-in-clear-text
RUN dotnet restore
COPY src .
WORKDIR /src/Web
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
