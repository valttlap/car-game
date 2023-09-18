# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CarGame.Api/CarGame.Api.csproj", "CarGame.Api/"]
COPY ["CarGame.Model/CarGame.Model.csproj", "CarGame.Model/"]
RUN dotnet restore "CarGame.Api/CarGame.Api.csproj"
COPY . .
WORKDIR "/src/CarGame.Api"
RUN dotnet build "CarGame.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarGame.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarGame.Api.dll"]
