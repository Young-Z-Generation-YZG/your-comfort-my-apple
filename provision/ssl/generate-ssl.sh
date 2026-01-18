#!/bin/bash

# Configuration
SSL_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
DAYS=365
SUBJ="/C=VN/ST=HCM/L=HCM/O=YGZ/OU=Project/CN=ybzone.io.vn"

echo "Generating self-signed SSL certificate in: $SSL_DIR"

openssl req -x509 -newkey rsa:4096 -keyout "$SSL_DIR/privkey.pem" -out "$SSL_DIR/fullchain.pem" -days $DAYS -nodes -subj "$SUBJ"

# Make sure permissions are correct for Docker/Nginx
chmod 644 "$SSL_DIR/privkey.pem"
chmod 644 "$SSL_DIR/fullchain.pem"

echo "Success! Certificates generated:"
ls -lh "$SSL_DIR"/*.pem
