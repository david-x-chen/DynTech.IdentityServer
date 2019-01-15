[![Build status](https://techpro.visualstudio.com/DynTech.IdentityServer/_apis/build/status/idsrv4_to_docker)](https://techpro.visualstudio.com/DynTech.IdentityServer/_build/latest?definitionId=1)
https://sonarqube.dynweb.biz/api/project_badges/measure?project=dyn-idsrv4&metric=alert_status
Run following commands in the same folder.

# Add migrations for ApplicationDbContext
dotnet ef migrations add UpdateApplicationUser -c ApplicationDbContext -o Data/Migrations

# Add migrations for IdentityServer4
dotnet ef migrations add Grants -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add Config -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb

# Update Database
dotnet ef Database update -c PersistedGrantDbContext
dotnet ef Database update -c ConfigurationDbContext


# set google api client id & secret (see msa-profile-google.json)
dotnet user-secrets set Authentication:Google:ClientID <client_id>
dotnet user-secrets set Authentication:Google:ClientSecret <client-secret>

# Deployed with nginx
https://docs.microsoft.com/en-us/aspnet/core/publishing/linuxproduction?tabs=aspnetcore2x
sudo systemctl restart kestrel-identityserver4.service