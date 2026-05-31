# Campus Clash — Backend

API REST para la plataforma de torneos de esports universitarios interfacultades.

## Stack tecnológico

- **Framework:** ASP.NET Core (.NET 10)
- **Lenguaje:** C#
- **Base de datos:** PostgreSQL
- **ORM:** Entity Framework Core
- **Autenticación:** JWT + BCrypt
- **Arquitectura:** Clean Architecture

## Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download)
- [Git](https://git-scm.com)

## Configuración inicial

### 1. Clonar el repositorio

```bash
git clone https://github.com/moreescudero/CampusClash.BackEnd.git
cd CampusClash.BackEnd
```

### 2. Configurar variables de entorno

Copiá el archivo de ejemplo y completá con tus datos:

```bash
cp CampusClash.API/appsettings.example.json CampusClash.API/appsettings.json
```

Editá `appsettings.json` con tus valores:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=campusclash;Username=tu_usuario;Password=tu_password"
  },
  "Jwt": {
    "Key": "tu-clave-secreta-minimo-32-caracteres",
    "Issuer": "CampusClash",
    "Audience": "CampusClash"
  },
  "RiotGames": {
    "ApiKey": "RGAPI-tu-api-key",
    "BaseUrl": "https://la1.api.riotgames.com"
  }
}
```

Para obtener tu ```Development Api Key``` entra al siguiente link: https://developer.riotgames.com/

### 3. Crear la base de datos

Asegurate de tener PostgreSQL corriendo y creá la base de datos:

```sql
CREATE DATABASE campusclash;
```

### 4. Aplicar migraciones

```bash
dotnet ef database update --project CampusClash.Infrastructure --startup-project CampusClash.API
```

### 5. Cargar datos iniciales

Ejecutá el script de universidades en pgAdmin o psql:

```sql
INSERT INTO "Universities" ("Id", "Name", "ShortName") VALUES
('00000000-0000-0000-0000-000000000001', 'Universidad Argentina de la Empresa', 'UADE'),
('00000000-0000-0000-0000-000000000002', 'Universidad de Buenos Aires', 'UBA'),
('00000000-0000-0000-0000-000000000003', 'Universidad Tecnológica Nacional', 'UTN');
-- Ver script completo en /docs/seed.sql
```

### 6. Correr el proyecto

```bash
dotnet run --project CampusClash.API
```

La API va a estar disponible en `http://localhost:5027/swagger`

## Estructura del proyecto
```
CampusClash.BackEnd/
├── CampusClash.API/              # Controllers, Program.cs
├── CampusClash.Application/      # Servicios, DTOs, Interfaces
├── CampusClash.Domain/           # Entidades, Enums
├── CampusClash.Infrastructure/   # Repositorios, DbContext
└── CampusClash.Tests/            # Tests unitarios
```

## Endpoints disponibles

### Autenticación
| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| POST | `/api/Auth/register` | Registro de usuario | ❌ |
| POST | `/api/Auth/login` | Login | ❌ |
| GET | `/api/Auth/confirm-email` | Confirmar email | ❌ |

### Validación académica
| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| POST | `/api/Validation/request` | Solicitar validación | ✅ |
| GET | `/api/Validation/status` | Consultar estado | ✅ |

### Riot Games
| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| POST | `/api/Riot/link` | Vincular cuenta Riot | ✅ |

## Sprints

### Sprint 1 — Base del sistema ✅
- Registro y autenticación con JWT
- Confirmación de cuenta por email
- Vinculación con Riot Games
- Trámite de validación como alumno regular

### Sprint 2 — Gestión de torneos 🔄
- Crear, editar y cancelar torneos
- Inscripción a torneos
- Ver mis torneos

### Sprint 3 — Cierre competitivo ⏳
- Generación de brackets
- Redirección a Riot y creación de sala
- Buscar torneos

## Autoras

- Morena Escudero — [GitHub](https://github.com/moreescudero) · [LinkedIn](https://linkedin.com/in/morena-escudero-7943ab221)
- Fiona Pardo