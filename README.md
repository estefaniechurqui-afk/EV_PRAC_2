# Lucy Fast Food - Backend

Este proyecto es el backend de Lucy Fast Food. Es una API REST creada con ASP.NET Core y SQLite.

El sistema permite administrar productos y ventas. También incluye un inicio de sesión para proteger las operaciones de administración.

## Tecnologías utilizadas

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT para la autenticación
- Swagger para probar la API

## Requisitos

Antes de ejecutar el proyecto debes tener instalado:

- .NET SDK 10
- Entity Framework Core CLI

Para comprobar la instalación de .NET:

```bash
dotnet --version
```

Para instalar la herramienta de Entity Framework:

```bash
dotnet tool install --global dotnet-ef --version 10.0.11
```

Si ya está instalada, puedes actualizarla con:

```bash
dotnet tool update --global dotnet-ef --version 10.0.11
```

## Base de datos

El proyecto utiliza SQLite. No es necesario instalar SQL Server, MySQL o un servidor adicional.

La base de datos se llama:

```text
LucyFastFood.db
```

El archivo se crea en la carpeta del proyecto cuando se ejecuta la migración.

## Configurar la base de datos

Desde la carpeta del proyecto ejecuta:

```bash
dotnet restore
dotnet ef database update
```

Si necesitas borrar la base local y crearla nuevamente:

```bash
dotnet ef database drop --force
dotnet ef database update
```

Este comando elimina todos los datos guardados localmente.

## Ejecutar el backend

Para iniciar el proyecto con HTTPS:

```bash
dotnet run --launch-profile https
```

La API estará disponible en:

```text
https://localhost:7244
```

Swagger estará disponible en:

```text
https://localhost:7244/swagger
```

Si el navegador muestra una advertencia sobre el certificado, se debe aceptar el certificado local de desarrollo.

## Usuario de prueba

El proyecto crea automáticamente un usuario de prueba cuando se ejecuta por primera vez:

```text
Usuario: admin
Contraseña: Admin123!
```

## Inicio de sesión

Para iniciar sesión se debe enviar una petición `POST` a:

```text
/api/auth/login
```

Ejemplo de cuerpo de la petición:

```json
{
  "usuario": "admin",
  "password": "Admin123!"
}
```

La respuesta contiene un token. Ese token debe enviarse en las rutas protegidas usando el encabezado:

```text
Authorization: Bearer TU_TOKEN
```

## Rutas de productos

Consultar todos los productos. No requiere inicio de sesión:

```text
GET /api/productos
```

Consultar un producto. No requiere inicio de sesión:

```text
GET /api/productos/{id}
```

Crear un producto. Requiere token:

```text
POST /api/productos
```

Ejemplo:

```json
{
  "nombre": "Hamburguesa clásica",
  "descripcion": "Hamburguesa con carne y queso",
  "precio": 25.50,
  "categoria": "Hamburguesas",
  "imagen": null,
  "disponible": true
}
```

Actualizar un producto. Requiere token:

```text
PUT /api/productos/{id}
```

Eliminar un producto. Requiere token:

```text
DELETE /api/productos/{id}
```

Un producto no puede eliminarse si tiene ventas relacionadas.

## Rutas de ventas

Todas las rutas de ventas requieren token.

Consultar todas las ventas:

```text
GET /api/ventas
```

Consultar una venta:

```text
GET /api/ventas/{id}
```

Crear una venta:

```text
POST /api/ventas
```

Ejemplo:

```json
{
  "productoId": 1,
  "cantidad": 2
}
```

El total se calcula automáticamente usando el precio del producto y la cantidad.

Actualizar una venta:

```text
PUT /api/ventas/{id}
```

Eliminar una venta:

```text
DELETE /api/ventas/{id}
```

## Migraciones

Cuando se cambie la estructura de los modelos, se debe crear una nueva migración:

```bash
dotnet ef migrations add NombreDeLaMigracion
dotnet ef database update
```

Las migraciones se guardan en la carpeta `Migrations`.

## Estructura principal

```text
Controllers/  Rutas de la API.
Data/         Configuración de Entity Framework.
DTOs/         Datos que reciben y responden los endpoints.
Models/       Entidades de la base de datos.
Migrations/   Cambios de la estructura de la base de datos.
Services/     Lógica de autenticación.
```

## Seguridad

La clave JWT incluida en `appsettings.json` es solo para desarrollo y pruebas locales. Antes de publicar el sistema en producción se debe cambiar por una variable de entorno y usar una clave privada.

## Estado del proyecto

El backend cuenta con:

- CRUD de productos.
- CRUD de ventas.
- Relación entre productos y ventas.
- Inicio de sesión.
- Autenticación mediante JWT.
- Protección de las rutas administrativas.
- CORS para permitir la conexión con un frontend.
- Documentación y pruebas mediante Swagger.
