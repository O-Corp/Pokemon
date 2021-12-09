FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY ./Pokemon.sln ./

# COPY src
COPY ./src/Rotomdex.Web.Api/Rotomdex.Web.Api.csproj ./src/Rotomdex.Web.Api/Rotomdex.Web.Api.csproj
COPY ./src/Rotomdex.Domain/Rotomdex.Domain.csproj ./src/Rotomdex.Domain/Rotomdex.Domain.csproj
COPY ./src/Rotomdex.Integration/Rotomdex.Integration.csproj ./src/Rotomdex.Integration/Rotomdex.Integration.csproj

# COPY tests
COPY ./tests/Rotomdex.Web.Api.UnitTests/Rotomdex.Web.Api.UnitTests.csproj ./tests/Rotomdex.Web.Api.UnitTests/Rotomdex.Web.Api.UnitTests.csproj
COPY ./tests/Rotomdex.Web.Api.ComponentTests/Rotomdex.Web.Api.ComponentTests.csproj ./tests/Rotomdex.Web.Api.ComponentTests/Rotomdex.Web.Api.ComponentTests.csproj
COPY ./tests/Rotomdex.Domain.UnitTests/Rotomdex.Domain.UnitTests.csproj ./tests/Rotomdex.Domain.UnitTests/Rotomdex.Domain.UnitTests.csproj
COPY ./tests/Rotomdex.Integration.UnitTests/Rotomdex.Integration.UnitTests.csproj ./tests/Rotomdex.Integration.UnitTests/Rotomdex.Integration.UnitTests.csproj
COPY ./tests/Rotomdex.Testing.Common/Rotomdex.Testing.Common.csproj ./tests/Rotomdex.Testing.Common/Rotomdex.Testing.Common.csproj
COPY ./tests/Rotomdex.IntegrationTests/Rotomdex.IntegrationTests.csproj ./tests/Rotomdex.IntegrationTests/Rotomdex.IntegrationTests.csproj

# Restore
RUN dotnet restore

COPY . .

RUN dotnet publish ./src/Rotomdex.Web.Api/Rotomdex.Web.Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Rotomdex.Web.Api.dll"]