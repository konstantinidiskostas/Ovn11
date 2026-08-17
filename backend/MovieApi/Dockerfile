# 1. Χρησιμοποιούμε το επίσημο runtime image της Microsoft για .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. Χρησιμοποιούμε το SDK image για να κάνουμε build τον κώδικα
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 3. Αντιγράφουμε το αρχείο του project και κάνουμε restore τα Nuget πακέτα
COPY ["MovieApi.csproj", "."]
RUN dotnet restore "./MovieApi.csproj"

# 4. Αντιγράφουμε όλο τον υπόλοιπο κώδικα και κάνουμε build
COPY . .
WORKDIR "/src/."
RUN dotnet build "MovieApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 5. Προετοιμάζουμε τα αρχεία για τη δημοσίευση (Publish)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MovieApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 6. Τελικό στάδιο: Παίρνουμε τα έτοιμα αρχεία και ορίζουμε πώς ξεκινάει η εφαρμογή
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieApi.dll"]

# ... (όλο το υπόλοιπο Dockerfile ίδιο)

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "MovieApi.dll"]