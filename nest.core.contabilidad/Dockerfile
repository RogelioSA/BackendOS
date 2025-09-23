FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nest.core.contabilidad/nest.core.contabilidad.csproj", "nest.core.contabilidad/"]
RUN dotnet restore "nest.core.contabilidad/nest.core.contabilidad.csproj"
COPY . .
WORKDIR "/src/nest.core.contabilidad"
RUN dotnet build "nest.core.contabilidad.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "nest.core.contabilidad.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "nest.core.contabilidad.dll"]
