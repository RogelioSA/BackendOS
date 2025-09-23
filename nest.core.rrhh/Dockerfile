FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nest.core.rrhh/nest.core.rrhh.csproj", "nest.core.rrhh/"]
RUN dotnet restore "nest.core.rrhh/nest.core.rrhh.csproj"
COPY . .
WORKDIR "/src/nest.core.rrhh"
RUN dotnet build "nest.core.rrhh.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "nest.core.rrhh.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "nest.core.rrhh.dll"]
