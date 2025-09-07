
## KC CLI
### export realm include users data
/opt/keycloak/bin/kc.sh export --dir /tmp/export  --realm ygz-realm   --users realm_file

**ygz-realm-realm.json**
- exported using cli above
note: remember to remove 2 default elements in array => key:"policies" (keep rest of policies you have created)