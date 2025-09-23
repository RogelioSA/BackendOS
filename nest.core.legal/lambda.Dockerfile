FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.legal/nest.core.legal.csproj", "nest.core.legal/"]
RUN dotnet restore "nest.core.legal/nest.core.legal.csproj"
COPY . .
WORKDIR /src/nest.core.legal
RUN dotnet publish "nest.core.legal.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.legal"]