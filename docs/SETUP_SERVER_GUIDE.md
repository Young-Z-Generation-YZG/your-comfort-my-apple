#

## SETUP

```sh
sudo apt udpate & sudo apt upgrade
```

## INSTALL DOCKER & DOCKER COMPOSE

```sh
sudo apt update

sudo apt install -y apt-transport-https ca-certificates curl software-properties-common

curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg

echo "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt update

sudo apt install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin

sudo apt-get update

sudo apt-get install docker-compose-plugin
```

## GENERATE SSL

```sh
sudo apt update

sudo apt install python3 python3-venv libaugeas0

sudo python3 -m venv /opt/certbot/

sudo /opt/certbot/bin/pip install --upgrade pip

sudo /opt/certbot/bin/pip install certbot certbot-nginx

sudo ln -s /opt/certbot/bin/certbot /usr/bin/certbot

sudo certbot certonly --standalone -d ybstore.io.vn -d admin.ybstore.io.vn -d keycloak.ybstore.io.vn -d logging.ybstore.io.vn -d tracking.ybstore.io.vn

sudo cp /etc/letsencrypt/live/ybstore.io.vn/fullchain.pem ~/repo/your-comfort-my-apple/provision/ssl/

sudo cp /etc/letsencrypt/live/ybstore.io.vn/privkey.pem ~/repo/your-comfort-my-apple/provision/ssl/
```
