FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as builder

WORKDIR /src/WordCounter.Api
COPY WordCounter/WordCounter.Api/WordCounter.Api.csproj .
RUN dotnet restore

COPY WordCounter/WordCounter.Api /src/WordCounter.Api
RUN dotnet publish -c Release -o /out WordCounter.Api.csproj

# app image
FROM  mcr.microsoft.com/dotnet/aspnet:5.0-alpine

EXPOSE 80
ENTRYPOINT ["dotnet", "/app/WordCounter.Api.dll"]

WORKDIR /app
COPY --from=builder /out/ .