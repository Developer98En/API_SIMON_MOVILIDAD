# System Design Documentation

## Overview
Este sistema implementa una plataforma de monitoreo vehicular en tiempo real que permite:
- Autenticación de usuarios con roles (ADMIN / USER)
- Gestión de dispositivos IoT
- Recepción de telemetría (ubicación, velocidad, combustible, temperatura)
- Generación de alertas predictivas
- Visualización en tiempo real mediante WebSockets (SignalR)

La arquitectura sigue un enfoque **API REST + comunicación en tiempo real**.

---

## Tech Stack Selection

### Backend
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **JWT Authentication**
- **SignalR**

#### Justificación
- ASP.NET Core ofrece alto rendimiento, tipado fuerte y buen soporte para APIs escalables.
- Entity Framework Core facilita el modelado relacional y migraciones.
- PostgreSQL es robusto, open-source y adecuado para datos relacionales.
- JWT permite autenticación stateless, ideal para APIs distribuidas.
- SignalR habilita comunicación en tiempo real para telemetría.

---

### Frontend
- **React (Vite)**
- **JavaScript**
- **Axios**
- **Google Maps / MapLibre**
- **Chart.js / Recharts**

#### Justificación
- React permite componentes reutilizables y manejo eficiente de estado.
- Vite acelera el entorno de desarrollo.
- Axios facilita interceptores para JWT.
- Librerías de mapas permiten visualización geográfica en tiempo real.
- Gráficos históricos mejoran el análisis de datos.

---

## Architecture

### High-level Architecture

[ IoT Device ]
|
v
[ Sensors API ] ---> PostgreSQL
|
v
[ SignalR Hub ] ---> Frontend Dashboard


### Controllers
- **AuthController**: Registro y login con JWT
- **DevicesController**: Gestión y asignación de dispositivos
- **SensorsController**: Ingesta de telemetría y generación de alertas
- **AlertsController**: Consulta y resolución de alertas
- **UsersController**: Gestión de usuarios (solo ADMIN)

---

## Data Model

### Relaciones Clave
- User ↔ Device (Many-to-Many)
- Device → SensorReadings (One-to-Many)
- Device → Alerts (One-to-Many)

Esto permite:
- Múltiples usuarios por dispositivo
- Historial completo de telemetría
- Alertas asociadas a cada vehículo

---

## Critical Business Logic

### Fuel Alert Logic
- Si `FuelLevel < 20`, se genera automáticamente una alerta predictiva.
- La alerta permanece activa hasta ser resuelta manualmente.

### Security
- Todas las rutas críticas requieren JWT.
- Validación de rol ADMIN para acciones sensibles (ej. listar usuarios).
- Tokens firmados con clave secreta y expiración.

---

## Trade-offs & Technical Decisions

### JWT vs Session-based Auth
✅ JWT elegido por:
- Escalabilidad
- Stateless
- Fácil integración con frontend

❌ Trade-off:
- No permite invalidación inmediata sin estrategia adicional (blacklist).

---

### SignalR vs REST Polling
✅ SignalR:
- Comunicación en tiempo real
- Menor latencia
- Mejor experiencia de usuario

❌ Trade-off:
- Mayor complejidad
- Manejo de conexiones concurrentes

---

### EF Core InMemory para Testing
✅ Permite pruebas rápidas y aisladas
❌ No detecta errores específicos del motor real (PostgreSQL)

---

## Testing Strategy
- **Unit Tests** para:
  - Autenticación JWT
  - Generación de alertas
  - Resolución de alertas
- Uso de `EF Core InMemory` y `xUnit`

---

## Future Improvements
- Políticas de autorización con `[Authorize]`
- Refresh tokens
- Alertas configurables por usuario
- Cache distribuido (Redis)
- Contenedorización con Docker
