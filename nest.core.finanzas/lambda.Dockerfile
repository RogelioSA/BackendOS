FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.finanzas/nest.core.finanzas.csproj", "nest.core.finanzas/"]
RUN dotnet restore "nest.core.finanzas/nest.core.finanzas.csproj"
COPY . .
WORKDIR /src/nest.core.finanzas
RUN dotnet publish "nest.core.finanzas.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.finanzas"]