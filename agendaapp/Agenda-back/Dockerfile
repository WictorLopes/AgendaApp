# Usa a imagem base do SDK do .NET 9.0 para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia os arquivos do projeto e restaura dependências
COPY . ./
RUN dotnet restore Agenda.API/Agenda.API.csproj

# Publica o projeto
RUN dotnet publish Agenda.API/Agenda.API.csproj -c Release -o /app/publish

# Usa a imagem runtime para execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Agenda.API.dll"]