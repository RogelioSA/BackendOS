FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.security/nest.core.security.csproj", "nest.core.security/"]
RUN dotnet restore "nest.core.security/nest.core.security.csproj"
COPY . .
WORKDIR /src/nest.core.security
RUN dotnet publish "nest.core.security.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.security"]