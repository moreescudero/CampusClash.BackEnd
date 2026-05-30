FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.sln .
COPY CampusClash.API/*.csproj ./CampusClash.API/
COPY CampusClash.Application/*.csproj ./CampusClash.Application/
COPY CampusClash.Domain/*.csproj ./CampusClash.Domain/
COPY CampusClash.Infrastructure/*.csproj ./CampusClash.Infrastructure/
COPY CampusClash.Tests/*.csproj ./CampusClash.Tests/

RUN dotnet restore

COPY . .

RUN dotnet publish CampusClash.API/CampusClash.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CampusClash.API.dll"]