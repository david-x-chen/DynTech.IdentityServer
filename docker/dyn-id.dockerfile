FROM microsoft/dotnet:latest

MAINTAINER David Chen

# Set environment variables
ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Development"

# Open up port
EXPOSE 5000

COPY src /app

WORKDIR /app

RUN dotnet restore

WORKDIR /app/DynTech.IdentityServer

RUN dotnet build

ENTRYPOINT ["dotnet", "run"]