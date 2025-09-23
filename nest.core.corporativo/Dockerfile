FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nest.core.corporativo/nest.core.corporativo.csproj", "nest.core.corporativo/"]
RUN dotnet restore "nest.core.corporativo/nest.core.corporativo.csproj"
COPY . .
WORKDIR "/src/nest.core.corporativo"
RUN dotnet build "nest.core.corporativo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "nest.core.corporativo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "nest.core.corporativo.dll"]
