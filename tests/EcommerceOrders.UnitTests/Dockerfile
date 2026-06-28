FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/EcommerceOrders.UnitTests/EcommerceOrders.UnitTests.csproj", "tests/EcommerceOrders.UnitTests/"]
COPY ["src/EcommerceOrders.Application/EcommerceOrders.Application.csproj", "src/EcommerceOrders.Application/"]
COPY ["src/EcommerceOrders.Domain/EcommerceOrders.Domain.csproj", "src/EcommerceOrders.Domain/"]
RUN dotnet restore "tests/EcommerceOrders.UnitTests/EcommerceOrders.UnitTests.csproj"
COPY . .
WORKDIR "/src/tests/EcommerceOrders.UnitTests"
RUN dotnet build "./EcommerceOrders.UnitTests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EcommerceOrders.UnitTests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EcommerceOrders.UnitTests.dll"]
