FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.rrhh/nest.core.rrhh.csproj", "nest.core.rrhh/"]
RUN dotnet restore "nest.core.rrhh/nest.core.rrhh.csproj"
COPY . .
WORKDIR /src/nest.core.rrhh
RUN dotnet publish "nest.core.rrhh.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.rrhh"]