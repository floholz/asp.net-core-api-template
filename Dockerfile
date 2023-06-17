FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["asp.net-core-api-template.csproj", "./"]
RUN dotnet restore "asp.net-core-api-template.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "asp.net-core-api-template.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "asp.net-core-api-template.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "asp.net-core-api-template.dll"]
