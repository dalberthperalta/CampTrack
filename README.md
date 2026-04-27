# CampTrack - Sistema de Inventario para Equipos de Camping Extremo
**Dalberth Josue Peralta Taveras | Programación II**

---

## 🚀 Cómo abrir y correr el proyecto

### Paso 1 — Abrir la solución
Haz doble clic en `CampTrack.sln` — Visual Studio 2022 abrirá todo automáticamente.

### Paso 2 — Restaurar paquetes NuGet
En Visual Studio: **Build → Restore NuGet Packages**  
O click derecho en la solución → **Restore NuGet Packages**

### Paso 3 — Verificar la cadena de conexión
Abre `CampTrack.API/appsettings.json`.  
Si tu SQL Server tiene usuario/contraseña cambia a:
```json
"Server=localhost;Database=CampTrackDb;User Id=sa;Password=TuPassword;TrustServerCertificate=True;"
```

### Paso 4 — Crear la base de datos (migraciones)
En Visual Studio: **Tools → NuGet Package Manager → Package Manager Console**

Escribe estos comandos uno por uno:
```
Add-Migration Init -Project CampTrack.Persistence -StartupProject CampTrack.API
Update-Database -Project CampTrack.Persistence -StartupProject CampTrack.API
```

### Paso 5 — Correr el proyecto
- Click derecho en `CampTrack.API` → **Set as Startup Project**
- Presiona **F5** o el botón verde ▶️
- Se abre Swagger en el navegador (la API está corriendo)
- Anota el puerto HTTPS (ej: `https://localhost:7175`)

### Paso 6 — Abrir el frontend
Abre el archivo `client-js/index.html` en tu navegador.  
Si el puerto de tu API es diferente a `7175`, edita la primera línea de `client-js/js/app.js`:
```javascript
const API_BASE_URL = 'https://localhost:TUPORTE';
```

---

## 📁 Estructura del Proyecto

```
CampTrack/
├── CampTrack.Domain/          ← Entidades (Equipo, Categoria, Prestamo, Revision)
├── CampTrack.Application/     ← Interfaces, DTOs, Servicios, AutoMapper
├── CampTrack.Infrastructure/  ← Repositorios + Unit of Work
├── CampTrack.Persistence/     ← DbContext + Migraciones + Seeder
├── CampTrack.API/             ← Controllers + Program.cs
└── client-js/                 ← Frontend HTML + CSS + JavaScript
```

## 🗄️ Endpoints de la API

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | /api/equipos | Listar todos los equipos |
| GET | /api/equipos/disponibles | Solo equipos disponibles |
| GET | /api/equipos/buscar?nombre=x | Buscar por nombre |
| POST | /api/equipos | Crear equipo |
| PUT | /api/equipos/{id} | Actualizar equipo |
| DELETE | /api/equipos/{id} | Eliminar equipo |
| GET | /api/categorias | Listar categorías |
| POST | /api/categorias | Crear categoría |
| PUT | /api/categorias/{id} | Actualizar categoría |
| DELETE | /api/categorias/{id} | Eliminar categoría |
| GET | /api/prestamos | Listar préstamos |
| GET | /api/prestamos/activos | Solo préstamos activos |
| POST | /api/prestamos | Crear préstamo |
| PUT | /api/prestamos/{id}/devolver | Marcar como devuelto |
| GET | /api/revisiones | Listar revisiones |
| POST | /api/revisiones | Crear revisión |
| DELETE | /api/revisiones/{id} | Eliminar revisión |
