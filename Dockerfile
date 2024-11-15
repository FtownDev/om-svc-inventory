FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apk update
RUN apk upgrade
RUN apk add curl
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /
COPY ["om-svc-inventory/om-svc-inventory.csproj", "./"]
RUN dotnet restore "om-svc-inventory.csproj"
COPY . .

FROM build AS publish
RUN dotnet publish "om-svc-inventory/om-svc-inventory.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "om-svc-inventory.dll"]