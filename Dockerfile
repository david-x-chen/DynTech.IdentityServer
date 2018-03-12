FROM microsoft/aspnetcore:latest

ARG DB_HOST="localhost"
# Set environment variables
ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Production"
ENV DB_HOST=$DB_HOST

ADD VERSION .

WORKDIR /app

COPY publish ./

ENTRYPOINT ["dotnet", "DynTech.IdentityServer.dll"]