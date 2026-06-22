#!/bin/bash
# Test directo a la LCU — corre desde tu PC con League abierto

LCU_ARGS=$(ps aux | grep LeagueClientUx | grep -v grep | head -1)
PORT=$(echo "$LCU_ARGS" | grep -o '\-\-app-port=[0-9]*' | cut -d= -f2)
TOKEN=$(echo "$LCU_ARGS" | grep -o '\-\-remoting-auth-token=[^ ]*' | cut -d= -f2)

[ -z "$PORT" ]  && echo "League no está abierto" && exit 1

echo "Puerto: $PORT"
echo "Token:  $TOKEN"
echo ""

# Escribir el body en un archivo temporal
cat > /tmp/lcu-body.json << 'EOF'
{
  "customGameLobby": {
    "configuration": {
      "gameMode": "CLASSIC",
      "mapId": 11,
      "teamSize": 5,
      "spectatorPolicy": "AllAllowed",
      "pickType": "",
      "customMutatorName": "SimulPickStrategy"
    },
    "lobbyName": "CampusClash Test",
    "lobbyPassword": ""
  },
  "isCustom": true,
  "queueId": 3100
}
EOF

echo "=== POST /lol-lobby/v2/lobby ==="
curl -k -s \
  -u "riot:${TOKEN}" \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -X POST "https://127.0.0.1:${PORT}/lol-lobby/v2/lobby" \
  -d @/tmp/lcu-body.json
echo ""
