FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.datasource/nest.core.datasource.csproj", "nest.core.datasource/"]
RUN dotnet restore "nest.core.datasource/nest.core.datasource.csproj"
COPY . .
WORKDIR /src/nest.core.datasource
RUN dotnet publish "nest.core.datasource.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.datasource"]
