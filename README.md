# Lucy Fast Food - Backend

Este proyecto contiene el backend de Lucy Fast Food. Es una API creada con ASP.NET Core, SQLite y JWT.

## Requisitos

Se necesita tener instalado:

- .NET SDK 10
- Entity Framework Core CLI

Si no tienes Entity Framework instalado:

```bash
dotnet tool install --global dotnet-ef --version 10.0.11
```

## Base de datos

El proyecto utiliza SQLite. La base de datos se llama:

```text
LucyFastFood.db
```

Para crear o actualizar la base de datos:

```bash
dotnet restore
dotnet ef database update
```

## Ejecutar el backend

```bash
dotnet run --launch-profile https
```

API:

```text
https://localhost:7244
```

Swagger:

```text
https://localhost:7244/swagger
```

## Usuario de prueba

```text
Usuario: admin
Contraseña: Admin123!
```

## Login

```text
POST /api/auth/login
```

Cuerpo de ejemplo:

```json
{
  "usuario": "admin",
  "password": "Admin123!"
}
```

El login devuelve un token. Para usar las rutas protegidas se debe enviar:

```text
Authorization: Bearer TU_TOKEN
```

## Rutas principales

Productos públicos:

```text
GET /api/productos
GET /api/productos/{id}
```

Productos protegidos:

```text
POST   /api/productos
PUT    /api/productos/{id}
DELETE /api/productos/{id}
```

Ventas protegidas:

```text
GET    /api/ventas
GET    /api/ventas/{id}
POST   /api/ventas
PUT    /api/ventas/{id}
DELETE /api/ventas/{id}
```

El total de una venta se calcula automáticamente con el precio del producto y la cantidad.

## Migraciones

Cuando se cambie la estructura de la base de datos:

```bash
dotnet ef migrations add NombreDeLaMigracion
dotnet ef database update
```

## Estructura

```text
Controllers/  Rutas de la API
Data/         Conexión con SQLite
DTOs/         Datos de entrada y salida
Models/       Modelos del sistema
Migrations/   Cambios de la base de datos
Services/     Login y autenticación
```

## Nota

La clave JWT incluida en el proyecto es para pruebas locales. Para producción se debe guardar en una variable de entorno.
