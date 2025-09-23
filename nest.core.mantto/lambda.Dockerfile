FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.mantto/nest.core.mantto.csproj", "nest.core.mantto/"]
RUN dotnet restore "nest.core.mantto/nest.core.mantto.csproj"
COPY . .
WORKDIR /src/nest.core.mantto
RUN dotnet publish "nest.core.mantto.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.mantto"]