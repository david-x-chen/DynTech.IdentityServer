set -ex

# SET THE FOLLOWING VARIABLES
# docker hub username
USERNAME=davidxchen
# image name
IMAGE=profile

#dotnet build

#dotnet publish -c release -o ~/Projects/DynTech.IdentityServer/publish -f netcoreapp2.0

docker build -t $USERNAME/$IMAGE:latest .