FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ARG APP_PORT=3000
ARG BUILD_COMMIT
ARG BUILD_NUMBER
ARG BUILD_ID

EXPOSE 3000
ENV ASPNETCORE_URLS=http://+:${APP_PORT}
ENV BUILD_COMMIT=$BUILD_COMMIT
ENV BUILD_NUMBER=$BUILD_NUMBER
ENV BUILD_ID=$BUILD_ID

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the necessary Directory.Build.props and Directory.Build.targets
COPY Directory.Build.props .
COPY Directory.Packages.props .

COPY ["src/Api/Api.csproj", "src/Api/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/Api/Api.csproj"
COPY . .

WORKDIR "src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

#RUN dotnet test "/test/Domain.UnitTests/Domain.UnitTests.csproj"

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuestSystem.Api.dll"]
