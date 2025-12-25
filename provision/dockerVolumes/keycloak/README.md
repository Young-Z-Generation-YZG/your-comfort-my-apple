## KC CLI

### export realm include users data

/opt/keycloak/bin/kc.sh export --dir /tmp/export --realm ygz-realm --users realm_file
`docker exec -it ygz.keycloak.server /opt/keycloak/bin/kc.sh export --dir /tmp/export  --realm ygz-realm   --users realm_file`

### copy from container to local machine

`docker cp ygz.keycloak.server:/tmp/export ./provision/dockerVolumes/keycloak`

**ygz-realm-realm.json**

-   exported using cli above
    note: remember to remove 2 default elements in array => key:"policies" (keep rest of policies you have created)

```json
...
    "authorizationSettings" : {
      "allowRemoteResourceManagement" : true,
      "policyEnforcementMode" : "ENFORCING",
      "resources" : [ {
        "name" : "Default Resource",
        "type" : "urn:admin-rest-api:resources:default",
        "ownerManagedAccess" : false,
        "attributes" : { },
        "uris" : [ "/*" ]
      } ],
      "policies" : [ {
        "name" : "Default Policy",
        "description" : "A policy that grants access only for users within this realm",
        "type" : "js",
        "logic" : "POSITIVE",
        "decisionStrategy" : "AFFIRMATIVE",
        "config" : {
          "code" : "// by default, grants any permission associated with this policy\n$evaluation.grant();\n"
        }
      }, {
        "name" : "Default Permission",
        "description" : "A permission that applies to the default resource type",
        "type" : "resource",
        "logic" : "POSITIVE",
        "decisionStrategy" : "UNANIMOUS",
        "config" : {
          "defaultResourceType" : "urn:admin-rest-api:resources:default",
          "applyPolicies" : "[\"Default Policy\"]"
        }
      } ],
      "scopes" : [ ],
      "decisionStrategy" : "UNANIMOUS"
    }
...
```

=> Delete 2 array items in policies ("Default Policy", "Default Permission")
