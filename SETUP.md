# Local Setup Guide

## Prerequisites
- .NET SDK 7+
- Node.js 18+
- PostgreSQL
- Git

---

## Backend Setup

### 1. Clone repository
```bash
git clone <repository-url>
cd Api

2. Configure Database

Crear una base de datos PostgreSQL y actualizar appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=telemetrydb;Username=postgres;Password=postgres"
  }
}

3. Run Migrations
dotnet ef database update

4. Run Backend
dotnet run


La API estará disponible en:

http://localhost:5068

Frontend Setup
1. Install dependencies
cd frontend
npm install

2. Configure API URL

En el archivo de configuración:

const API_URL = "http://localhost:5068/api";

3. Run Frontend
npm run dev


La aplicación estará disponible en:

http://localhost:5173

Authentication Flow

Usuario se autentica vía /api/auth/login

Se recibe JWT

El token se guarda en localStorage

Axios interceptor adjunta el token a cada request

Real-time Telemetry

El frontend se conecta al TelemetryHub

Cada dispositivo se une a su grupo por DeviceId

Las actualizaciones se envían vía telemetryUpdate

Running Tests
dotnet test


Incluye pruebas para:

Autenticación

Alertas predictivas

Sensores

Common Issues
Token inválido

Verificar header:

Authorization: Bearer <token>

SignalR no recibe datos

Verificar que el frontend se una al grupo correcto

Validar DeviceId

Environment Notes

El sistema usa UTC para timestamps

JWT expira en 60 minutos

Roles soportados: ADMIN, USER
