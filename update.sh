#!/bin/bash

docker-compose down
docker pull ghcr.io/markhuang1212/aka
docker pull cloudflare/cloudflared:2021.11.0
docker-compose up -d