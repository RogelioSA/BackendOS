FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.logistica/nest.core.logistica.csproj", "nest.core.logistica/"]
RUN dotnet restore "nest.core.logistica/nest.core.logistica.csproj"
COPY . .
WORKDIR /src/nest.core.logistica
RUN dotnet publish "nest.core.logistica.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.logistica"]