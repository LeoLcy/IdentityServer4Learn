#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Leo.LearnIdentityServer4.Server.EFStore/Leo.LearnIdentityServer4.Server.EFStore.csproj", "Leo.LearnIdentityServer4.Server.EFStore/"]
RUN dotnet restore "Leo.LearnIdentityServer4.Server.EFStore/Leo.LearnIdentityServer4.Server.EFStore.csproj"
COPY . .
WORKDIR "/src/Leo.LearnIdentityServer4.Server.EFStore"
RUN dotnet build "Leo.LearnIdentityServer4.Server.EFStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Leo.LearnIdentityServer4.Server.EFStore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Leo.LearnIdentityServer4.Server.EFStore.dll"]