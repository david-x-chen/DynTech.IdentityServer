FROM microsoft/aspnetcore:latest

ARG DB_HOST="localhost"
ARG DB_REPLICA=""
# Set environment variables
ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Production"
ENV DB_HOST=$DB_HOST
ENV DB_REPLICA=$DB_REPLICA

ADD VERSION .

WORKDIR /app

COPY publish ./

ENTRYPOINT ["dotnet", "DynTech.IdentityServer.dll"]