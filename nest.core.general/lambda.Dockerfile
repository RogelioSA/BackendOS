FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.general/nest.core.general.csproj", "nest.core.general/"]
RUN dotnet restore "nest.core.general/nest.core.general.csproj"
COPY . .
WORKDIR /src/nest.core.general
RUN dotnet publish "nest.core.general.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.general"]