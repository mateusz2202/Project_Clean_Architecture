#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HH_ASP_APP/HH_ASP_APP.csproj", "HH_ASP_APP/"]
RUN dotnet restore "HH_ASP_APP/HH_ASP_APP.csproj"
COPY . .
WORKDIR "/src/HH_ASP_APP"
RUN dotnet build "HH_ASP_APP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HH_ASP_APP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HH_ASP_APP.dll"]