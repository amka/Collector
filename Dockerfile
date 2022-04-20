# Declate runtime container
FROM alpine:latest AS base

# Add some libs required by .NET runtime 
RUN apk add --no-cache libstdc++ libintl

WORKDIR /app
EXPOSE 5183

ENV ASPNETCORE_URLS=http://+:5183
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Declare build container
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["Collector.csproj", "./"]
RUN dotnet restore "Collector.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Collector.csproj" -c Release -o /app/build

# Build Stage
FROM build AS publish
RUN dotnet publish "Collector.csproj" -c Release -o /app/publish -r alpine-x64 --self-contained true /p:PublishTrimmed=true

# Publish Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./Collector"]
