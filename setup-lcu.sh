#!/bin/bash

# ─────────────────────────────────────────────────────────────────────────────
# CampusClash — LCU Setup Script
# Uso: ./setup-lcu.sh
#
# Requisitos:
#   brew install ngrok/ngrok/ngrok    (o npm install -g ngrok)
#   brew install jq
# ─────────────────────────────────────────────────────────────────────────────

API_URL="https://campusclashbackend-production.up.railway.app"   # ← URL del backend en Railway (sin barra final)

# ─── Colores ───────────────────────────────────────────────────────────────
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
BOLD='\033[1m'
NC='\033[0m'

header() {
  echo ""
  echo -e "${BLUE}${BOLD}╔══════════════════════════════════════════╗${NC}"
  echo -e "${BLUE}${BOLD}║      CampusClash — LCU Setup             ║${NC}"
  echo -e "${BLUE}${BOLD}╚══════════════════════════════════════════╝${NC}"
  echo ""
}

ok()   { echo -e "${GREEN}  ✓ $1${NC}"; }
info() { echo -e "${CYAN}  → $1${NC}"; }
warn() { echo -e "${YELLOW}  ⚠ $1${NC}"; }
fail() { echo -e "${RED}  ✗ $1${NC}"; exit 1; }

# ─── Verificar dependencias ─────────────────────────────────────────────────
check_deps() {
  command -v ngrok &>/dev/null || fail "ngrok no está instalado. Corré: brew install ngrok/ngrok/ngrok"
  command -v jq    &>/dev/null || fail "jq no está instalado. Corré: brew install jq"
  command -v curl  &>/dev/null || fail "curl no está disponible."

  # Verificar que ngrok tenga authtoken configurado
  local config_file="${HOME}/Library/Application Support/ngrok/ngrok.yml"
  if [ ! -f "$config_file" ] || ! grep -q "authtoken" "$config_file"; then
    echo ""
    echo -e "${RED}${BOLD}  ngrok requiere autenticación.${NC}"
    echo ""
    echo -e "  1. Creá una cuenta gratis en:"
    echo -e "     ${CYAN}https://dashboard.ngrok.com/signup${NC}"
    echo ""
    echo -e "  2. Copiá tu authtoken de:"
    echo -e "     ${CYAN}https://dashboard.ngrok.com/get-started/your-authtoken${NC}"
    echo ""
    echo -e "  3. Ejecutá este comando y volvé a correr el script:"
    echo -e "     ${YELLOW}ngrok config add-authtoken TU_TOKEN_ACÁ${NC}"
    echo ""
    exit 1
  fi
  ok "ngrok autenticado"
}

# ─── Esperar que League esté corriendo ──────────────────────────────────────
wait_for_league() {
  info "Esperando que League of Legends esté corriendo..."
  local dots=0
  while true; do
    LCU_ARGS=$(ps aux | grep LeagueClientUx | grep -v grep | head -1)
    [ -n "$LCU_ARGS" ] && break
    printf "."
    ((dots++))
    [ $dots -gt 60 ] && echo "" && fail "Tiempo de espera agotado. ¿Está abierto League of Legends?"
    sleep 2
  done
  echo ""
  ok "League of Legends detectado"
}

# ─── Extraer puerto y token ──────────────────────────────────────────────────
extract_lcu_credentials() {
  LCU_PORT=$(echo "$LCU_ARGS" | grep -o '\-\-app-port=[0-9]*' | cut -d= -f2)
  LCU_TOKEN=$(echo "$LCU_ARGS" | grep -o '\-\-remoting-auth-token=[^ ]*' | cut -d= -f2)

  [ -z "$LCU_PORT" ]  && fail "No se pudo extraer el puerto de LeagueClientUx."
  [ -z "$LCU_TOKEN" ] && fail "No se pudo extraer el auth token de LeagueClientUx."

  ok "Puerto LCU: ${LCU_PORT}"
  ok "Token LCU extraído"
}

# ─── Iniciar ngrok ───────────────────────────────────────────────────────────
start_ngrok() {
  info "Cerrando instancias previas de ngrok..."
  pkill -f ngrok 2>/dev/null
  sleep 1

  info "Iniciando ngrok en puerto ${LCU_PORT}..."
  ngrok http "https://127.0.0.1:${LCU_PORT}" \
    --host-header="127.0.0.1:${LCU_PORT}" \
    --log=stdout > /tmp/campusclash-ngrok.log 2>&1 &
  NGROK_PID=$!

  # Esperar hasta 10s a que ngrok levante
  local attempts=0
  while [ $attempts -lt 10 ]; do
    NGROK_URL=$(curl -s http://localhost:4040/api/tunnels 2>/dev/null \
      | jq -r '.tunnels[] | select(.proto=="https") | .public_url' 2>/dev/null)
    [ -n "$NGROK_URL" ] && break
    sleep 1
    ((attempts++))
  done

  if [ -z "$NGROK_URL" ]; then
    echo ""
    echo -e "${RED}  Error de ngrok:${NC}"
    grep -E "eror|crit|ERROR" /tmp/campusclash-ngrok.log | tail -5 | sed 's/^/    /'
    echo ""
    fail "ngrok no pudo iniciar. Revisá los errores de arriba."
  fi

  ok "ngrok URL: ${NGROK_URL}"
}

# ─── Login automático ────────────────────────────────────────────────────────
login() {
  echo ""
  echo -e "${BOLD}  Credenciales de CampusClash:${NC}"
  echo ""
  read -p "  📧 Email: " CC_EMAIL
  read -s -p "  🔒 Contraseña: " CC_PASSWORD
  echo ""

  info "Iniciando sesión en el backend..."

  LOGIN_RESPONSE=$(curl -s -L -o /tmp/campusclash-login.json -w "%{http_code}" \
    -X POST "${API_URL}/api/auth/login" \
    -H "Content-Type: application/json" \
    -d "{\"email\": \"${CC_EMAIL}\", \"password\": \"${CC_PASSWORD}\"}")

  if [ "$LOGIN_RESPONSE" != "200" ]; then
    local body
    body=$(cat /tmp/campusclash-login.json 2>/dev/null)
    fail "Error al iniciar sesión (HTTP ${LOGIN_RESPONSE}): ${body}"
  fi

  JWT_TOKEN=$(jq -r '.token' /tmp/campusclash-login.json 2>/dev/null)
  [ -z "$JWT_TOKEN" ] || [ "$JWT_TOKEN" = "null" ] && fail "No se pudo extraer el token del response de login."

  ok "Sesión iniciada correctamente"
}

# ─── Pedir Match ID ──────────────────────────────────────────────────────────
prompt_match_id() {
  echo ""
  read -p "  📋 Match ID (Guid del partido) [5e5e5e5e-0000-0000-0000-000000000001]: " MATCH_ID
  MATCH_ID="${MATCH_ID:-5e5e5e5e-0000-0000-0000-000000000001}"
}

# ─── Registrar sesión en el backend ─────────────────────────────────────────
register_session() {
  info "Registrando sesión LCU en el backend..."

  RESPONSE=$(curl -s -L -o /tmp/campusclash-register.json -w "%{http_code}" \
    -X POST "${API_URL}/api/lcu/register" \
    -H "Content-Type: application/json" \
    -H "Authorization: Bearer ${JWT_TOKEN}" \
    -d "{\"matchId\": \"${MATCH_ID}\", \"baseUrl\": \"${NGROK_URL}\", \"authToken\": \"${LCU_TOKEN}\"}")

  if [ "$RESPONSE" = "200" ]; then
    ok "Sesión registrada exitosamente en el backend"
  else
    local body
    body=$(cat /tmp/campusclash-register.json 2>/dev/null)
    fail "Error HTTP ${RESPONSE}: ${body}"
  fi
}

# ─── Resumen final ───────────────────────────────────────────────────────────
print_summary() {
  echo ""
  echo -e "${GREEN}${BOLD}  ══════════════════════════════════════════${NC}"
  echo -e "${GREEN}${BOLD}    ¡Todo listo! Próximos pasos:${NC}"
  echo -e "${GREEN}${BOLD}  ══════════════════════════════════════════${NC}"
  echo ""
  echo -e "  ${CYAN}1. Crear el lobby en League:${NC}"
  echo -e "     POST ${API_URL}/api/matches/${MATCH_ID}/create-lobby"
  echo ""
  echo -e "  ${CYAN}2. Invitar jugadores:${NC}"
  echo -e "     POST ${API_URL}/api/matches/${MATCH_ID}/invite"
  echo -e "     Body: { \"summonerNames\": [\"jugador1\", \"jugador2\", ...] }"
  echo ""
  echo -e "  ${CYAN}3. Ver quién se unió al lobby:${NC}"
  echo -e "     GET  ${API_URL}/api/matches/${MATCH_ID}/lobby-status"
  echo ""
  echo -e "${YELLOW}  ngrok corriendo (PID: ${NGROK_PID}). No cierres esta terminal.${NC}"
  echo -e "${YELLOW}  Para cerrar ngrok cuando termines: ${RED}pkill -f ngrok${NC}"
  echo ""
  echo -e "  ${CYAN}Log de ngrok:${NC} /tmp/campusclash-ngrok.log"
  echo ""
}

# ─── Crear lobby directo desde la LCU local ──────────────────────────────────
create_lobby_local() {
  echo ""
  read -p "  ¿Crear el lobby en League ahora? [s/N]: " CREAR
  [[ ! "$CREAR" =~ ^[sS]$ ]] && return

  cat > /tmp/lcu-lobby-body.json << EOF
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
    "lobbyName": "CampusClash - Match ${MATCH_ID}",
    "lobbyPassword": ""
  },
  "isCustom": true,
  "queueId": 3100
}
EOF

  info "Creando lobby en League of Legends..."
  LOBBY_RESP=$(curl -k -s -o /tmp/lcu-lobby-resp.json -w "%{http_code}" \
    -u "riot:${LCU_TOKEN}" \
    -H "Content-Type: application/json" \
    -H "Accept: application/json" \
    -X POST "https://127.0.0.1:${LCU_PORT}/lol-lobby/v2/lobby" \
    -d @/tmp/lcu-lobby-body.json)

  if [ "$LOBBY_RESP" = "200" ]; then
    ok "¡Lobby creado! Aparece en tu cliente de League."

    # Notificar al backend
    curl -s -L -X POST "${API_URL}/api/matches/${MATCH_ID}/lobby-created" \
      -H "Authorization: Bearer ${JWT_TOKEN}" > /dev/null

    invite_players_local
  else
    echo -e "${RED}  Error creando lobby (HTTP ${LOBBY_RESP}):${NC}"
    cat /tmp/lcu-lobby-resp.json
    echo ""
  fi
}

# ─── Invitar jugadores directo desde la LCU local ────────────────────────────
invite_players_local() {
  echo ""
  read -p "  ¿Invitar jugadores ahora? [s/N]: " INVITAR
  [[ ! "$INVITAR" =~ ^[sS]$ ]] && return

  read -p "  Nombres de invocador separados por coma: " NAMES_RAW
  IFS=',' read -ra NAMES <<< "$NAMES_RAW"

  local INVITATIONS="["
  local FIRST=true

  for NAME in "${NAMES[@]}"; do
    NAME=$(echo "$NAME" | xargs)  # trim spaces
    ENCODED=$(python3 -c "import urllib.parse; print(urllib.parse.quote('${NAME}'))" 2>/dev/null || \
              node -e "process.stdout.write(encodeURIComponent('${NAME}'))" 2>/dev/null || \
              echo "$NAME")

    SUMMONER_RESP=$(curl -k -s \
      -u "riot:${LCU_TOKEN}" \
      -H "Accept: application/json" \
      "https://127.0.0.1:${LCU_PORT}/lol-summoner/v1/summoners?name=${ENCODED}")

    SUMMONER_ID=$(echo "$SUMMONER_RESP" | jq -r '.summonerId // empty' 2>/dev/null)

    if [ -n "$SUMMONER_ID" ]; then
      ok "Invocador encontrado: ${NAME} (ID: ${SUMMONER_ID})"
      $FIRST || INVITATIONS+=","
      INVITATIONS+="{\"toSummonerId\":${SUMMONER_ID}}"
      FIRST=false
    else
      warn "No se encontró el invocador: ${NAME}"
    fi
  done

  INVITATIONS+="]"

  if [ "$INVITATIONS" = "[]" ]; then
    warn "No se encontró ningún invocador válido."
    return
  fi

  info "Enviando invitaciones..."
  echo "$INVITATIONS" > /tmp/lcu-invites.json
  INV_RESP=$(curl -k -s -o /tmp/lcu-inv-resp.json -w "%{http_code}" \
    -u "riot:${LCU_TOKEN}" \
    -H "Content-Type: application/json" \
    -X POST "https://127.0.0.1:${LCU_PORT}/lol-lobby/v2/lobby/invitations" \
    -d @/tmp/lcu-invites.json)

  [ "$INV_RESP" = "200" ] && ok "¡Invitaciones enviadas!" || \
    warn "Error enviando invitaciones (HTTP ${INV_RESP})"
}

# ─── Main ────────────────────────────────────────────────────────────────────
header
check_deps
wait_for_league
extract_lcu_credentials
start_ngrok
login
prompt_match_id
register_session
create_lobby_local
print_summary

# Mantener ngrok vivo hasta que se apriete Ctrl+C
echo -e "${YELLOW}  Presioná Ctrl+C para cerrar ngrok y salir${NC}"
trap "echo ''; warn 'Cerrando ngrok...'; pkill -f ngrok; exit 0" SIGINT
wait $NGROK_PID
