FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Mock.Content.Distribution/Mock.Content.Distribution.csproj", "Mock.Content.Distribution/"]
RUN dotnet restore "Mock.Content.Distribution/Mock.Content.Distribution.csproj"
COPY . .
WORKDIR "/src/Mock.Content.Distribution"
RUN dotnet build "Mock.Content.Distribution.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Mock.Content.Distribution.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mock.Content.Distribution.dll"]
