FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Aseta.sln", "."]
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

COPY ["src/Aseta.API/Aseta.API.csproj", "src/Aseta.API/"]
COPY ["src/Aseta.Application/Aseta.Application.csproj", "src/Aseta.Application/"]
COPY ["src/Aseta.Domain/Aseta.Domain.csproj", "src/Aseta.Domain/"]
COPY ["src/Aseta.Infrastructure/Aseta.Infrastructure.csproj", "src/Aseta.Infrastructure/"]

RUN dotnet restore "Aseta.sln"

COPY . .
WORKDIR "/src/src/Aseta.API"
RUN dotnet build "Aseta.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet publish "src/Aseta.API/Aseta.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aseta.API.dll"]
