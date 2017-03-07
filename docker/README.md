
### Docker 

##### Local - (Build container)

```bash
cd  DynTech.IdentityServer/

docker build -f docker/dyn-id.dockerfile -t dyn-identityserver  .
##docker-compose --file docker/docker-compose-local.yml  up
```

##### Staging 

```bash
cd  DynTech.IdentityServer/docker

##docker-compose --file docker-compose-staging.yml  up
````


