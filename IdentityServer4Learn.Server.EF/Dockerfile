FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["IdentityServer4Learn.Server.EF/IdentityServer4Learn.Server.EF.csproj", "IdentityServer4Learn.Server.EF/"]
RUN dotnet restore "IdentityServer4Learn.Server.EF/IdentityServer4Learn.Server.EF.csproj"
COPY . .
WORKDIR "/src/IdentityServer4Learn.Server.EF"
RUN dotnet build "IdentityServer4Learn.Server.EF.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IdentityServer4Learn.Server.EF.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IdentityServer4Learn.Server.EF.dll"]