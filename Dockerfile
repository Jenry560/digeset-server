FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["digeset-server.Api/digeset-server.Api.csproj", "digeset-server.Api/"]
COPY ["digeset-server.Application/digeset-server.Application.csproj", "digeset-server.Application/"]
COPY ["digeset-server.Core/digeset-server.Core.csproj", "digeset-server.Core/"]
COPY ["digeset-server.Infrastructure/digeset-server.Infrastructure.csproj", "digeset-server.Infrastructure/"]
RUN dotnet restore "digeset-server.Api/digeset-server.Api.csproj"
COPY . .
WORKDIR "/src/digeset-server.Api"
RUN dotnet build "digeset-server.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "digeset-server.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "digeset-server.Api.dll"]
