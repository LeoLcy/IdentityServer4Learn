FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["IdentityServer4Learn.Api/IdentityServer4Learn.Api.csproj", "IdentityServer4Learn.Api/"]
RUN dotnet restore "IdentityServer4Learn.Api/IdentityServer4Learn.Api.csproj"
COPY . .
WORKDIR "/src/IdentityServer4Learn.Api"
RUN dotnet build "IdentityServer4Learn.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IdentityServer4Learn.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IdentityServer4Learn.Api.dll"]