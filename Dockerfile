FROM node:latest as build-node

WORKDIR /app
COPY Client/package.json Client/package-lock.json ./
RUN npm install
COPY Client/ .
RUN npm run build -- --output-path=./dist/out

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-net
WORKDIR /app

COPY ["Backend/CarGame.Model/CarGame.Model.csproj", "./Backend/CarGame.Model/"]
COPY ["Backend/CarGame.Api/CarGame.Api.csproj", "./Backend/CarGame.Api/"]
RUN dotnet restore "Backend/CarGame.Api/CarGame.Api.csproj"

COPY ["Backend/CarGame.Model/", "./Backend/CarGame.Model/"]
COPY ["Backend/CarGame.Api/", "./Backend/CarGame.Api/"]
COPY --from=build-node /app/dist/out ./Backend/CarGame.Api/wwwroot
RUN dotnet publish Backend/CarGame.Api/CarGame.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-net /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "CarGame.Api.dll"]