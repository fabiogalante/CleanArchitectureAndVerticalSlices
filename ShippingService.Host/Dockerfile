FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShippingService.Host/ShippingService.Host.csproj", "ShippingService.Host/"]
COPY ["ShippingService.Domain/ShippingService.Domain.csproj", "ShippingService.Domain/"]
COPY ["ShippingService.Infrastructure/ShippingService.Infrastructure.csproj", "ShippingService.Infrastructure/"]
COPY ["ShippingService.Features/ShippingService.Features.csproj", "ShippingService.Features/"]
RUN dotnet restore "ShippingService.Host/ShippingService.Host.csproj"
COPY . .
WORKDIR "/src/ShippingService.Host"
RUN dotnet build "ShippingService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ShippingService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShippingService.Host.dll"]
