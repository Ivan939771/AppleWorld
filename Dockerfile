# Multi-stage Dockerfile for ASP.NET Core 8 Web Application on Render
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["APPLEWORLD/AppleWorldShoppingMallEMS.csproj", "APPLEWORLD/"]
RUN dotnet restore "APPLEWORLD/AppleWorldShoppingMallEMS.csproj"

COPY APPLEWORLD/ APPLEWORLD/
WORKDIR "/src/APPLEWORLD"
RUN dotnet build "AppleWorldShoppingMallEMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppleWorldShoppingMallEMS.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p wwwroot/uploads
ENTRYPOINT ["dotnet", "AppleWorldShoppingMallEMS.dll"]
