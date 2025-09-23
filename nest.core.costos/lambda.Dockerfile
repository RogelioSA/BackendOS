FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.costos/nest.core.costos.csproj", "nest.core.costos/"]
RUN dotnet restore "nest.core.costos/nest.core.costos.csproj"
COPY . .
WORKDIR /src/nest.core.costos
RUN dotnet publish "nest.core.costos.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.costos"]