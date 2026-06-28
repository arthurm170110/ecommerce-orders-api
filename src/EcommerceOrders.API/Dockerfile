FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/EcommerceOrders.API/EcommerceOrders.API.csproj", "src/EcommerceOrders.API/"]
COPY ["src/EcommerceOrders.Infrastructure/EcommerceOrders.Infrastructure.csproj", "src/EcommerceOrders.Infrastructure/"]
COPY ["src/EcommerceOrders.Application/EcommerceOrders.Application.csproj", "src/EcommerceOrders.Application/"]
COPY ["src/EcommerceOrders.Domain/EcommerceOrders.Domain.csproj", "src/EcommerceOrders.Domain/"]
RUN dotnet restore "src/EcommerceOrders.API/EcommerceOrders.API.csproj"
COPY . .
WORKDIR "/src/src/EcommerceOrders.API"
RUN dotnet build "./EcommerceOrders.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EcommerceOrders.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EcommerceOrders.API.dll"]
