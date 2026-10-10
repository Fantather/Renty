FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Renty.Domain/Renty.Domain.csproj Renty.Domain/
COPY Renty.Application/Renty.Application.csproj Renty.Application/
COPY Renty.Infrastructure/Renty.Infrastructure.csproj Renty.Infrastructure/
COPY Renty.Web/Renty.Web.csproj Renty.Web/
RUN dotnet restore Renty.Web/Renty.Web.csproj

COPY . .
RUN dotnet publish Renty.Web/Renty.Web.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Renty.Web.dll"]
