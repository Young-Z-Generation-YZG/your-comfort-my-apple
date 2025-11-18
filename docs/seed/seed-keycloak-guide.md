# EXPORT

## Using Docker CLI

**1. export specific realm and users**:
`docker exec -it ygz.keycloak.server /opt/keycloak/bin/kc.sh export --dir /tmp/export --realm ygz-realm --users realm_file`

**2. copy from docker container to local machine**:
`docker cp ygz.keycloak.server:/tmp/export ./provision/dockerVolumes/keycloak`

## Note

-   Remember to remove 2 default elements in array => key:"client"[3]|[4]|[8]."authorizationSettings"."policies" (keep rest of policies you have created)
