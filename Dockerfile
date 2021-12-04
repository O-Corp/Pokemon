FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY ./Pokemon.sln ./
COPY ./src/Rotomdex.Web.Api/Rotomdex.Web.Api.csproj ./src/Rotomdex.Web.Api/Rotomdex.Web.Api.csproj
#COPY ./tests/Rotomdex.Web.Api.ComponentTests/Rotomdex.Web.Api.ComponentTests.csproj ./tests/Rotomdex.Web.Api.ComponentTests/Rotomdex.Web.Api.ComponentTests.csproj
#COPY ./tests/Rotomdex.Web.Api.UnitTests/Rotomdex.Web.Api.UnitTests.csproj ./tests/Rotomdex.Web.Api.UnitTests/Rotomdex.Web.Api.UnitTests.csproj
RUN dotnet restore

COPY . .

RUN dotnet publish ./Rotomdex.Web.Api/Rotomdex.Web.Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Rotomdex.Web.Api.dll"]