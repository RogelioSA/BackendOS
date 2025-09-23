FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["nest.core.corporativo/nest.core.corporativo.csproj", "nest.core.corporativo/"]
RUN dotnet restore "nest.core.corporativo/nest.core.corporativo.csproj"
COPY . .
WORKDIR /src/nest.core.corporativo
RUN dotnet publish "nest.core.corporativo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:PublishReadyToRun=true

FROM public.ecr.aws/lambda/dotnet:9 AS final
WORKDIR /var/task
COPY --from=build /app/publish ./

CMD ["nest.core.corporativo"]