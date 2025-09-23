FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.contabilidad/nest.core.contabilidad.csproj", "nest.core.contabilidad/"]
RUN dotnet restore "nest.core.contabilidad/nest.core.contabilidad.csproj"
COPY . .
WORKDIR /src/nest.core.contabilidad
RUN dotnet publish "nest.core.contabilidad.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.contabilidad"]