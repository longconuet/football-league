#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BetFootballLeague.WebUI/BetFootballLeague.WebUI.csproj", "BetFootballLeague.WebUI/"]
COPY ["BetFootballLeague.Application/BetFootballLeague.Application.csproj", "BetFootballLeague.Application/"]
COPY ["BetFootballLeague.Infrastructure/BetFootballLeague.Infrastructure.csproj", "BetFootballLeague.Infrastructure/"]
COPY ["BetFootballLeague.Domain/BetFootballLeague.Domain.csproj", "BetFootballLeague.Domain/"]
COPY ["BetFootballLeague.Shared/BetFootballLeague.Shared.csproj", "BetFootballLeague.Shared/"]
RUN dotnet restore "BetFootballLeague.WebUI/BetFootballLeague.WebUI.csproj"
COPY . .
WORKDIR "/src/BetFootballLeague.WebUI"
RUN dotnet build "BetFootballLeague.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BetFootballLeague.WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BetFootballLeague.WebUI.dll"]