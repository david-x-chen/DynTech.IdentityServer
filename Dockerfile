# Build Stage
FROM microsoft/dotnet:2.1-sdk AS build-env

COPY src /app/src
COPY DynTech.IdentityServer.sln /app/DynTech.IdentityServer.sln
#COPY NuGet.config /app/NuGet.config
WORKDIR /app

RUN dotnet restore

WORKDIR /app/src/DynTech.IdentityServer
RUN dotnet publish -o /publish -c Release -f netcoreapp2.1 -r debian.9-x64


# Runtime Image Stage
FROM microsoft/dotnet:2.1-aspnetcore-runtime

ARG DB_HOST="localhost"
ARG DB_REPLICA=""
ARG DB_USER=""
ARG DB_PWD=""
# Set environment variables
ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Docker"
ENV DB_HOST=$DB_HOST
ENV DB_REPLICA=$DB_REPLICA
ENV DB_USER=$DB_USER
ENV DB_PWD=$DB_PWD

ADD VERSION .

WORKDIR /publish
COPY --from=build-env /publish .

ENTRYPOINT ["dotnet", "DynTech.IdentityServer.dll"]