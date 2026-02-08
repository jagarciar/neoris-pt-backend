# Neoris Backend API

API REST desarrollada con **ASP.NET Web API (.NET Framework 4.8)** para gestionar autores y libros. Implementado siguiendo principios de **Clean Architecture** y **SOLID**.

## 🚀 Características

- **ASP.NET Web API** (.NET Framework 4.8)
- **RESTful API** con versionado (v1)
- **Swagger** para documentación interactiva
- **CORS** habilitado para todos los orígenes
- **Autenticación JWT** (Bearer Token)
- **Entity Framework 6.4.4** con SQL Server
- **Operaciones CRUD completas** para Autores y Libros
- **Validaciones de negocio** (ej: límite máximo de libros)
- Compatible con Windows Server / IIS
- ✅ **Arquitectura Limpia (Clean Architecture)**
- ✅ **Inyección de Dependencias (Unity)**
- ✅ **Repository Pattern y Unit of Work**
- ✅ **DTOs para contratos de API**
- ✅ **Logging con Serilog**
- ✅ **Principios SOLID implementados**

## 📦 Conexión a Base de Datos

La aplicación se conecta a **SQL Server 2022** ejecutándose en Docker:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=localhost,1433;Database=NeorisPTDB;User Id=sa;Password=Neoris2026!;TrustServerCertificate=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Nota**: Asegúrate de que SQL Server esté corriendo en Docker antes de ejecutar el backend. Ver instrucciones en el README principal del proyecto.

## 📁 Estructura del Proyecto

```
neoris-pt-backend/
│
├── Controllers/              # 🎮 Capa de Presentación
│   ├── AuthController.cs     # Maneja autenticación
│   ├── LibrosController.cs   # Maneja operaciones de libros
│   └── AutoresController.cs  # Maneja operaciones de autores
│
├── DTOs/                     # 📦 Data Transfer Objects
│   ├── Requests/             # DTOs para peticiones entrantes
│   │   ├── LibroCreateDto.cs
│   │   ├── LibroUpdateDto.cs
│   │   ├── AutorCreateDto.cs
│   │   ├── AutorUpdateDto.cs
│   │   └── LoginRequestDto.cs
│   └── Responses/            # DTOs para respuestas salientes
│       ├── LibroResponseDto.cs
│       ├── AutorResponseDto.cs
│       ├── LoginResponseDto.cs
│       └── ApiResponseDto.cs
│
├── Services/                 # 💼 Capa de Lógica de Negocio
│   ├── Interfaces/
│   │   ├── ILibroService.cs
│   │   ├── IAutorService.cs
│   │   └── IAuthService.cs
│   └── Implementations/
│       ├── LibroService.cs
│       ├── AutorService.cs
│       └── AuthService.cs
│
├── Repositories/             # 🗄️ Capa de Acceso a Datos
│   ├── Interfaces/
│   │   ├── IRepository.cs       # Repositorio genérico
│   │   ├── ILibroRepository.cs
│   │   ├── IAutorRepository.cs
│   │   └── IUnitOfWork.cs       # Patrón Unit of Work
│   └── Implementations/
│       ├── Repository.cs
│       ├── LibroRepository.cs
│       ├── AutorRepository.cs
│       └── UnitOfWork.cs
│
├── Models/                   # 🏛️ Entidades de Dominio
│   ├── Libro.cs
│   └── Autor.cs
│
├── Data/                     # 🗃️ Contexto de EF
│   └── NeorisPTDbContext.cs
│
├── Configuration/            # ⚙️ Configuraciones
│   ├── JwtConfig.cs
│   └── DatabaseConfig.cs
│
├── Logging/                  # 📝 Filtros y Logging
│   ├── GlobalExceptionFilter.cs
│   ├── RequestLoggingHandler.cs
│   └── ValidateModelStateFilter.cs
│
├── Extensions/               # 🔧 Métodos de Extensión
│   ├── StringExtensions.cs
│   └── EnumerableExtensions.cs
│
├── App_Start/               # 🚀 Configuración de Inicio
│   ├── WebApiConfig.cs
│   ├── SwaggerConfig.cs
│   └── UnityConfig.cs       # ⭐ Configuración de DI
│
├── Properties/
│   └── AssemblyInfo.cs           # Información del ensamblado
├── Global.asax                   # Punto de entrada de la aplicación
├── Global.asax.cs                # Código del Global.asax
├── Web.config                    # Configuración de la aplicación
├── packages.config               # Paquetes NuGet
└── neoris-pt-backend.csproj      # Archivo del proyecto
```

## 🔧 Requisitos

- .NET Framework 4.8 o superior
- Visual Studio 2019/2022 (recomendado)
- IIS Express o IIS para hosting
- SQL Server 2022 (ejecutándose en Docker)

## 🔐 Autenticación JWT

La API utiliza **JSON Web Tokens (JWT)** para autenticación. Todos los endpoints excepto `/api/v1/auth/login` requieren un token válido.

### Configuración en Web.config

```xml
<appSettings>
  <add key="JwtIssuer" value="neorisptbackend" />
  <add key="JwtAudience" value="neorisptbackend" />
  <add key="JwtSecret" value="NeorisJwt2026SuperSecretKey12345" />
  <add key="JwtExpirationSeconds" value="3600" />
  <add key="AuthUsername" value="neoris-pt-frontend" />
  <add key="AuthPassword" value="SecurePassword2026#NeorisSecure" />
</appSettings>
```

### Credenciales de Acceso

Para obtener un token JWT, usa estas credenciales:

- **Usuario**: `neoris-pt-frontend`
- **Contraseña**: `SecurePassword2026#NeorisSecure`
- **Endpoint**: `POST /api/v1/auth/login`

### Ejemplo de Autenticación

**Request:**
```http
POST http://localhost:5000/api/v1/auth/login
Content-Type: application/json

{
  "username": "neoris-pt-frontend",
  "password": "SecurePassword2026#NeorisSecure"
}
```

**Response:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresAtUtc": "2026-02-08T21:30:00Z"
}
```

### Uso del Token

Incluye el token en el header `Authorization` de todas las peticiones:

```http
GET http://localhost:5000/api/v1/autores
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📡 Endpoints de la API

### 🔓 Autenticación

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| POST | `/api/v1/auth/login` | Obtener token JWT | ❌ No |
| GET | `/api/v1/auth/me` | Info del usuario autenticado | ✅ Sí |

### 📚 Autores

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| GET | `/api/v1/autores` | Listar todos los autores | ✅ Sí |
| GET | `/api/v1/autores/{id}` | Obtener un autor por ID | ✅ Sí |
| POST | `/api/v1/autores` | Crear un nuevo autor | ✅ Sí |
| PUT | `/api/v1/autores/{id}` | Actualizar un autor | ✅ Sí |
| DELETE | `/api/v1/autores/{id}` | Eliminar un autor | ✅ Sí |

### 📖 Libros

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| GET | `/api/v1/libros` | Listar todos los libros | ✅ Sí |
| GET | `/api/v1/libros/{id}` | Obtener un libro por ID | ✅ Sí |
| POST | `/api/v1/libros` | Crear un nuevo libro | ✅ Sí |
| PUT | `/api/v1/libros/{id}` | Actualizar un libro | ✅ Sí |
| DELETE | `/api/v1/libros/{id}` | Eliminar un libro | ✅ Sí |

## ⚙️ Reglas de Negocio

### Validación de Límite de Libros

La aplicación valida un **límite máximo de libros** que pueden ser creados. Este límite se configura en `Web.config`:

```xml
<add key="MaxLibros" value="100" />
```

**Comportamiento:**
- Antes de crear un libro, el sistema verifica el total de libros existentes
- Si se alcanzó el límite, retorna `400 Bad Request` con el mensaje:
  ```json
  {
    "message": "No se puede crear el libro. Se ha alcanzado el límite máximo de 100 libros permitidos."
  }
  ```
- El límite se puede modificar cambiando el valor en `Web.config`

### Validación de Autor al Crear Libro

Al crear o actualizar un libro, se valida que el `AutorId` exista en la base de datos:

```json
{
  "message": "El autor con Id 999 no existe"
}
```

### Validación de Email Único en Autores

No se permiten autores con el mismo email:

```json
{
  "message": "Ya existe un autor con el email especificado"
}
```

## 🏛️ Capas de la Arquitectura

### 1️⃣ **Controllers (Capa de Presentación)**
**Responsabilidad**: Recibir peticiones HTTP y retornar respuestas

```csharp
public class LibrosController : ApiController
{
    private readonly ILibroService _libroService;

    public LibrosController(ILibroService libroService)
    {
        _libroService = libroService; // ✅ Inyección de Dependencias
    }

    [HttpGet]
    public IHttpActionResult GetLibros()
    {
        var libros = _libroService.GetAll(); // ✅ Delega a servicio
        return Ok(libros);
    }
}
```

**Características**:
- ❌ NO contienen lógica de negocio
- ✅ Solo orquestan llamadas a servicios
- ✅ Manejan ResponseTypes (200, 404, 400, etc.)
- ✅ Reciben DTOs, no entidades de dominio

---

### 2️⃣ **Services (Capa de Lógica de Negocio)**
**Responsabilidad**: Implementar reglas de negocio

```csharp
public class LibroService : ILibroService
{
    private readonly IUnitOfWork _unitOfWork;

    public LibroService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public LibroResponseDto Create(LibroCreateDto dto)
    {
        // ✅ Validación de negocio
        if (!_unitOfWork.Autores.Any(a => a.Id == dto.AutorId))
            throw new InvalidOperationException("El autor no existe");

        // ✅ Mapeo de DTO a Entidad
        var libro = new Libro { /* ... */ };
        
        // ✅ Operación sobre repositorio
        _unitOfWork.Libros.Add(libro);
        _unitOfWork.SaveChanges();

        return GetById(libro.Id);
    }
}
```

**Características**:
- ✅ Contienen toda la lógica de negocio
- ✅ Validan reglas de dominio
- ✅ Mapean entre DTOs y Entidades
- ✅ Usan repositorios, no DbContext directamente

---

### 3️⃣ **Repositories (Capa de Acceso a Datos)**
**Responsabilidad**: Abstraer acceso a datos

```csharp
public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    bool Any(Expression<Func<T, bool>> predicate);
}

public class Repository<T> : IRepository<T>
{
    protected readonly NeorisPTDbContext _context;
    protected readonly DbSet<T> _dbSet;

    // ✅ Implementación genérica reutilizable
}
```

**Patrón Unit of Work**:
```csharp
public interface IUnitOfWork
{
    ILibroRepository Libros { get; }
    IAutorRepository Autores { get; }
    int SaveChanges();
}
```

**Características**:
- ✅ Ocultam implementación de EF
- ✅ Facilitan testing (fácil de mockear)
- ✅ Centralizan consultas a BD
- ✅ Unit of Work coordina múltiples repositorios

---

### 4️⃣ **DTOs (Data Transfer Objects)**
**Responsabilidad**: Contratos de API separados del dominio

```csharp
// ✅ Request DTO
public class LibroCreateDto
{
    [Required]
    public string Titulo { get; set; }
    
    [Range(1, 9999)]
    public int Anio { get; set; }
    
    [Required]
    public int AutorId { get; set; }
}

// ✅ Response DTO
public class LibroResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public AutorResponseDto Autor { get; set; }
}
```

**Ventajas**:
- ✅ Evitan sobre-exponer entidades de dominio
- ✅ Permiten versionar API sin cambiar el dominio
- ✅ Reducen payload (solo datos necesarios)
- ✅ Validación específica por operación

---

## 🔄 Patrones Implementados

### 🎯 **1. Repository Pattern**
Abstrae la capa de datos

### 🎯 **2. Unit of Work Pattern**
Coordina transacciones entre múltiples repositorios

### 🎯 **3. Dependency Injection**
Inyección de dependencias con Unity

### 🎯 **4. Separation of Concerns**
Cada clase tiene una única responsabilidad

### 🎯 **5. DTO Pattern**
Contratos de API separados del dominio

---

## 🔄 Flujo de Datos

```
┌─────────────┐
│   Cliente   │
└──────┬──────┘
       │ HTTP Request (JSON)
       ▼
┌──────────────────────────────────────────┐
│         CONTROLLER                       │
│  ✅ Recibe LibroCreateDto                │
│  ✅ Valida ModelState                    │
│  ✅ Llama a _libroService.Create(dto)    │
└──────┬───────────────────────────────────┘
       │
       ▼
┌──────────────────────────────────────────┐
│         SERVICE                          │
│  ✅ Valida reglas de negocio             │
│  ✅ Mapea DTO → Entidad                  │
│  ✅ Llama a _unitOfWork.Libros.Add()     │
│  ✅ Llama a _unitOfWork.SaveChanges()    │
│  ✅ Mapea Entidad → DTO Response         │
└──────┬───────────────────────────────────┘
       │
       ▼
┌──────────────────────────────────────────┐
│       REPOSITORY                         │
│  ✅ _dbSet.Add(entity)                   │
│  ✅ _context.SaveChanges()               │
└──────┬───────────────────────────────────┘
       │
       ▼
┌──────────────────────────────────────────┐
│       DATABASE (SQL Server)              │
│  ✅ INSERT INTO Libros ...               │
└──────────────────────────────────────────┘
```

---

## 💉 Inyección de Dependencias

### Configuración en `UnityConfig.cs`

```csharp
public static void RegisterComponents()
{
    var container = new UnityContainer();

    // DbContext (una instancia por request)
    container.RegisterType<NeorisPTDbContext>(
        new HierarchicalLifetimeManager());

    // Unit of Work
    container.RegisterType<IUnitOfWork, UnitOfWork>(
        new HierarchicalLifetimeManager());

    // Repositorios
    container.RegisterType<ILibroRepository, LibroRepository>();
    container.RegisterType<IAutorRepository, AutorRepository>();

    // Servicios
    container.RegisterType<ILibroService, LibroService>();
    container.RegisterType<IAutorService, AutorService>();
    container.RegisterType<IAuthService, AuthService>();

    // Configurar resolver
    GlobalConfiguration.Configuration.DependencyResolver = 
        new UnityDependencyResolver(container);
}
```

### Llamado en `Global.asax.cs`

```csharp
protected void Application_Start()
{
    ConfigureLogging();
    UnityConfig.RegisterComponents(); // ⭐ Registrar DI
    GlobalConfiguration.Configure(WebApiConfig.Register);
}
```

---

## 🎓 Principios SOLID Aplicados

### **S** - Single Responsibility
Cada clase tiene una única responsabilidad:
- Controllers → Orquestación HTTP
- Services → Lógica de negocio
- Repositories → Acceso a datos

### **O** - Open/Closed
Abierto a extensión (nuevas servicios) pero cerrado a modificación

### **L** - Liskov Substitution
Interfaces permiten sustituir implementaciones

### **I** - Interface Segregation
Interfaces específicas (ILibroService, IAutorService)

### **D** - Dependency Inversion
Dependemos de abstracciones (interfaces), no de implementaciones concretas

---

## 🏃 Ejecución del Backend

### ⚠️ Requisitos Previos

Antes de ejecutar el backend, asegúrate de:

1. **SQL Server esté corriendo en Docker**
   ```powershell
   # Desde la raíz del proyecto (c:\Users\jeoga\Documents\Neoris\)
   .\start-stack.ps1
   
   # O manualmente:
   docker-compose up -d
   ```

2. **Verificar que la base de datos existe**
   ```powershell
   docker exec neoris-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Neoris2026!" -C -Q "SELECT name FROM sys.databases WHERE name='NeorisPTDB'"
   ```

### Opción 1: Visual Studio (Recomendado)

1. **Abrir el proyecto**
   - Abre `Neoris.sln` en Visual Studio 2019/2022
   - O solo el proyecto: `neoris-pt-backend.csproj`

2. **Establecer como proyecto de inicio**
   - Clic derecho en `neoris-pt-backend` → **Set as Startup Project**

3. **Restaurar paquetes NuGet**
   - Clic derecho en la solución → **Restore NuGet Packages**
   - O: `Tools` → `NuGet Package Manager` → `Package Manager Console`
   ```powershell
   Update-Package -Reinstall
   ```

4. **Compilar el proyecto**
   - `Build` → `Build Solution` (Ctrl+Shift+B)

5. **Ejecutar**
   - Presiona **F5** (con debugging) o **Ctrl+F5** (sin debugging)
   - El navegador abrirá automáticamente: `http://localhost:5000`
   - Accede a Swagger: `http://localhost:5000/swagger`

### Opción 2: Línea de Comandos

```powershell
# Navegar al directorio del backend
cd "c:\Users\jeoga\Documents\Neoris\neoris-pt-backend"

# Restaurar paquetes NuGet
nuget restore neoris-pt-backend.csproj

# Compilar el proyecto
msbuild neoris-pt-backend.csproj /p:Configuration=Release /p:Platform="Any CPU"

# Ejecutar con IIS Express (requiere IIS Express instalado)
"C:\Program Files\IIS Express\iisexpress.exe" /path:"%CD%" /port:5000
```

### Verificar que el Backend está corriendo

```bash
# Verificar endpoint de salud (si existe)
curl http://localhost:5000/api/v1/auth/me

# O navegar en el navegador a:
# http://localhost:5000/swagger
```

### Solución de Problemas

**Error: "The underlying provider failed on Open"**
- ✅ Verifica que SQL Server esté corriendo: `docker ps`
- ✅ Verifica la conexión: `docker exec neoris-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Neoris2026!" -C -Q "SELECT 1"`

**Error: "IDX10603: Decryption failed"**
- ✅ Verifica que la clave JWT en `Web.config` sea válida (mínimo 32 caracteres)
- ✅ Actual clave: `NeorisJwt2026SuperSecretKey12345`

**Error: "Could not load file or assembly"**
- ✅ Restaura los paquetes NuGet: `nuget restore`
- ✅ Limpia y recompila: `Clean Solution` → `Rebuild Solution`

**Error al crear libros: "Límite alcanzado"**
- ✅ Verifica/modifica `MaxLibros` en `Web.config`
- ✅ Actual límite: `100` libros

## ⚙️ Compilación y Ejecución (Detalle)

### Requisitos previos

1. **Visual Studio 2019 o 2022** con:
  - .NET Framework 4.8 Developer Pack
  - ASP.NET and web development workload

2. **Alternativa (solo compilación):**
  - .NET Framework 4.8 SDK
  - MSBuild Tools para Visual Studio

### Opción 1: Visual Studio (recomendado)

#### Paso 1: Abrir el proyecto
```
1. Haz doble clic en neoris-pt-backend.sln
2. O abre Visual Studio → File → Open → Project/Solution
3. Selecciona neoris-pt-backend.sln
```

#### Paso 2: Restaurar paquetes NuGet
```
1. Clic derecho en la solución → Restore NuGet Packages
2. O simplemente compila (F5), se restaurarán automáticamente
```

#### Paso 3: Ejecutar
```
1. Presiona F5 para ejecutar con debugging
2. O Ctrl+F5 para ejecutar sin debugging
3. La aplicación se abrirá en tu navegador predeterminado
4. Ve a http://localhost:{port}/swagger para ver la documentación
```

### Opción 2: Línea de comandos (requiere Build Tools)

#### Abrir Developer Command Prompt:
```
1. Busca "Developer Command Prompt for VS 2022" en el menú inicio
2. O "Developer PowerShell for VS 2022"
```

#### Restaurar y compilar:
```cmd
cd C:\Users\jeoga\Documents\Neoris\neoris-pt-backend
nuget restore neoris-pt-backend.sln
msbuild neoris-pt-backend.sln /p:Configuration=Release
```

## 🔍 Acceder a la aplicación

Una vez ejecutándose, accede a:

- **Home**: `http://localhost:5000/`
- **Swagger UI**: `http://localhost:5000/swagger`
- **API Autores**: `http://localhost:5000/api/v1/autores`
- **API Libros**: `http://localhost:5000/api/v1/libros`

## 📡 Endpoints Disponibles

### Autenticacion

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/v1/auth/login` | Emite un JWT |
| GET | `/api/v1/auth/me` | Devuelve el usuario autenticado |

### Autores

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/v1/autores` | Obtener todos los autores |
| GET | `/api/v1/autores/{id}` | Obtener autor por ID |
| POST | `/api/v1/autores` | Crear nuevo autor |
| PUT | `/api/v1/autores/{id}` | Actualizar autor |
| DELETE | `/api/v1/autores/{id}` | Eliminar autor |

### Libros

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/v1/libros` | Obtener todos los libros |
| GET | `/api/v1/libros/{id}` | Obtener libro por ID |
| POST | `/api/v1/libros` | Crear nuevo libro |
| PUT | `/api/v1/libros/{id}` | Actualizar libro |
| DELETE | `/api/v1/libros/{id}` | Eliminar libro |

## 📝 Ejemplos de Uso

### 1️⃣ Autenticación - Login (POST)

**Request:**
```bash
curl -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "neoris-pt-frontend",
    "password": "SecurePassword2026#NeorisSecure"
  }'
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresAtUtc": "2026-02-08T21:30:00Z"
}
```

### 2️⃣ Obtener todos los autores (GET)

**Request:**
```bash
curl -X GET http://localhost:5000/api/v1/autores \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "nombre": "Gabriel Garcia Marquez",
    "fechaNacimiento": "1927-03-06",
    "ciudadProcedencia": "Aracataca",
    "email": "gabriel.garcia@neoris.com",
    "fechaCreacion": "2026-02-08T10:00:00Z",
    "fechaModificacion": null
  },
  {
    "id": 2,
    "nombre": "Isabel Allende",
    "fechaNacimiento": "1942-08-02",
    "ciudadProcedencia": "Lima",
    "email": "isabel.allende@neoris.com",
    "fechaCreacion": "2026-02-08T10:00:00Z",
    "fechaModificacion": null
  }
]
```

### 3️⃣ Obtener un autor por ID (GET)

**Request:**
```bash
curl -X GET http://localhost:5000/api/v1/autores/1 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

**Response (200 OK):**
```json
{
  "id": 1,
  "nombre": "Gabriel Garcia Marquez",
  "fechaNacimiento": "1927-03-06",
  "ciudadProcedencia": "Aracataca",
  "email": "gabriel.garcia@neoris.com",
  "fechaCreacion": "2026-02-08T10:00:00Z",
  "fechaModificacion": null
}
```

### 4️⃣ Crear un autor (POST)

**Request:**
```bash
curl -X POST http://localhost:5000/api/v1/autores \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -d '{
    "nombre": "Mario Vargas Llosa",
    "fechaNacimiento": "1936-03-28",
    "ciudadProcedencia": "Arequipa",
    "email": "mario.vargas@neoris.com"
  }'
```

**Response (201 Created):**
```json
{
  "id": 6,
  "nombre": "Mario Vargas Llosa",
  "fechaNacimiento": "1936-03-28",
  "ciudadProcedencia": "Arequipa",
  "email": "mario.vargas@neoris.com",
  "fechaCreacion": "2026-02-08T15:30:00Z",
  "fechaModificacion": null
}
```

**Headers:**
```
Location: http://localhost:5000/api/v1/autores/6
```

### 5️⃣ Actualizar un autor (PUT)

**Request:**
```bash
curl -X PUT http://localhost:5000/api/v1/autores/6 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -d '{
    "nombre": "Mario Vargas Llosa (Actualizado)",
    "fechaNacimiento": "1936-03-28",
    "ciudadProcedencia": "Lima",
    "email": "mario.vargas.updated@neoris.com"
  }'
```

**Response (200 OK):**
```json
{
  "id": 6,
  "nombre": "Mario Vargas Llosa (Actualizado)",
  "fechaNacimiento": "1936-03-28",
  "ciudadProcedencia": "Lima",
  "email": "mario.vargas.updated@neoris.com",
  "fechaCreacion": "2026-02-08T15:30:00Z",
  "fechaModificacion": "2026-02-08T16:00:00Z"
}
```

### 6️⃣ Eliminar un autor (DELETE)

**Request:**
```bash
curl -X DELETE http://localhost:5000/api/v1/autores/6 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

**Response (200 OK):**
```json
{
  "message": "Autor 6 eliminado exitosamente"
}
```

### 7️⃣ Crear un libro (POST)

**Request:**
```bash
curl -X POST http://localhost:5000/api/v1/libros \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -d '{
    "titulo": "El coronel no tiene quien le escriba",
    "anio": 1961,
    "genero": "Novela",
    "numeroPaginas": 104,
    "autorId": 1
  }'
```

**Response (201 Created):**
```json
{
  "id": 6,
  "titulo": "El coronel no tiene quien le escriba",
  "anio": 1961,
  "genero": "Novela",
  "numeroPaginas": 104,
  "autorId": 1,
  "autor": {
    "id": 1,
    "nombre": "Gabriel Garcia Marquez",
    "fechaNacimiento": "1927-03-06",
    "ciudadProcedencia": "Aracataca",
    "email": "gabriel.garcia@neoris.com",
    "fechaCreacion": "2026-02-08T10:00:00Z",
    "fechaModificacion": null
  }
}
```

### 8️⃣ Error: Límite de libros alcanzado

**Request:**
```bash
curl -X POST http://localhost:5000/api/v1/libros \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -d '{
    "titulo": "Nuevo libro",
    "anio": 2026,
    "genero": "Ficción",
    "numeroPaginas": 300,
    "autorId": 1
  }'
```

**Response (400 Bad Request):**
```json
{
  "message": "No se puede crear el libro. Se ha alcanzado el límite máximo de 100 libros permitidos."
}
```

### 9️⃣ Error: Autor no existe

**Request:**
```bash
curl -X POST http://localhost:5000/api/v1/libros \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -d '{
    "titulo": "Libro sin autor",
    "anio": 2026,
    "genero": "Ficción",
    "numeroPaginas": 300,
    "autorId": 999
  }'
```

**Response (400 Bad Request):**
```json
{
  "message": "El autor con Id 999 no existe"
}
```
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -d '{
    "nombre": "Jorge Luis Borges",
    "fechaNacimiento": "1899-08-24",
    "ciudadProcedencia": "Buenos Aires",
    "email": "jorge.borges@neoris.com"
  }'
```

**Response (201 Created):**
```json
{
  "id": 3,
  "nombre": "Jorge Luis Borges",
  "fechaNacimiento": "1899-08-24",
  "ciudadProcedencia": "Buenos Aires",
  "email": "jorge.borges@neoris.com"
}
```

### 5️⃣ Actualizar un autor (PUT)

**Request:**
```bash
curl -X PUT http://localhost:5000/api/v1/autores/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -d '{
    "nombre": "Gabriel Garcia Marquez",
    "fechaNacimiento": "1927-03-06",
    "ciudadProcedencia": "Aracataca",
    "email": "gabriel.updated@neoris.com"
  }'
```

**Response (200 OK):**
```json
{
  "id": 1,
  "nombre": "Gabriel Garcia Marquez",
  "fechaNacimiento": "1927-03-06",
  "ciudadProcedencia": "Aracataca",
  "email": "gabriel.updated@neoris.com"
}
```

### 6️⃣ Eliminar un autor (DELETE)

**Request:**
```bash
curl -X DELETE http://localhost:5000/api/v1/autores/3 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

**Response (204 No Content):**
```
(sin cuerpo de respuesta)
```

### 7️⃣ Crear un libro (POST)

**Request:**
```bash
curl -X POST http://localhost:5000/api/v1/libros \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -d '{
    "titulo": "Cien años de soledad",
    "año": 1967,
    "autorId": 1,
    "descripcion": "Una novela épica que narra la historia de la familia Buendía"
  }'
```

**Response (201 Created):**
```json
{
  "id": 1,
  "titulo": "Cien años de soledad",
  "año": 1967,
  "autorId": 1,
  "autor": {
    "id": 1,
    "nombre": "Gabriel Garcia Marquez"
  },
  "descripcion": "Una novela épica que narra la historia de la familia Buendía"
}
```

### 8️⃣ Obtener todos los libros (GET)

**Request:**
```bash
curl -X GET http://localhost:5000/api/v1/libros \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "titulo": "Cien años de soledad",
    "año": 1967,
    "autorId": 1,
    "autor": {
      "id": 1,
      "nombre": "Gabriel Garcia Marquez"
    }
  },
  {
    "id": 2,
    "titulo": "Don Quijote",
    "año": 1605,
    "autorId": 2,
    "autor": {
      "id": 2,
      "nombre": "Miguel de Cervantes"
    }
  }
]
```

### 9️⃣ Obtener un libro por ID (GET)

**Request:**
```bash
curl -X GET http://localhost:5000/api/v1/libros/1 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

**Response (200 OK):**
```json
{
  "id": 1,
  "titulo": "Cien años de soledad",
  "año": 1967,
  "autorId": 1,
  "autor": {
    "id": 1,
    "nombre": "Gabriel Garcia Marquez"
  }
}
```

### 🔟 Actualizar un libro (PUT)

**Request:**
```bash
curl -X PUT http://localhost:5000/api/v1/libros/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
  -d '{
    "titulo": "Cien años de soledad (Edición actualizada)",
    "año": 1967,
    "autorId": 1
  }'
```

**Response (200 OK):**
```json
{
  "id": 1,
  "titulo": "Cien años de soledad (Edición actualizada)",
  "año": 1967,
  "autorId": 1,
  "autor": {
    "id": 1,
    "nombre": "Gabriel Garcia Marquez"
  }
}
```

### 1️⃣1️⃣ Eliminar un libro (DELETE)

**Request:**
```bash
curl -X DELETE http://localhost:5000/api/v1/libros/2 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

**Response (204 No Content):**
```
(sin cuerpo de respuesta)
```

### 🔧 Notas sobre los ejemplos:

- **Token JWT**: Todos los endpoints (excepto `/auth/login`) requieren autenticación
- **Obtener el token**: Ejecuta primero el endpoint de login para obtener un token válido
- **Reemplazar {id}**: Cambia los ID por valores reales en tu base de datos
- **Códigos de estado**:
  - `200 OK` - Operación exitosa
  - `201 Created` - Recurso creado exitosamente
  - `204 No Content` - Eliminación exitosa
  - `400 Bad Request` - Datos inválidos
  - `401 Unauthorized` - Token no válido o expirado
  - `404 Not Found` - Recurso no encontrado


## 🔍 Documentación Swagger

Una vez que la aplicación esté corriendo, puedes acceder a Swagger UI en:

- **Swagger UI**: `http://localhost:5000/swagger`
- **Swagger JSON**: `http://localhost:5000/swagger/docs/v1`

Swagger está preconfigurado y se carga automáticamente al iniciar la aplicación.

## 🌐 Pruebas

Puedes probar los endpoints usando:
- **curl**
- **Postman**
- **Fiddler**
- **Thunder Client** (extensión de VS Code)
- **REST Client** (extensión de VS Code) con el archivo `api-tests.http`
- **Swagger UI** (incluido en el proyecto)

## 🌐 Despliegue en IIS

Para publicar en IIS:

1. En Visual Studio, clic derecho en el proyecto → **Publish**
2. Selecciona **IIS, FTP, etc.**
3. Configura la ruta de publicación
4. Publica el proyecto
5. Configura un Application Pool en IIS con .NET Framework 4.8
6. Asigna el sitio web a la carpeta publicada

## 📋 Resumen de Configuraciones

### Web.config - Configuraciones Importantes

| Configuración | Valor | Descripción |
|---------------|-------|-------------|
| **JwtSecret** | `NeorisJwt2026SuperSecretKey12345` | Clave secreta para JWT (mín. 32 chars) |
| **JwtIssuer** | `neorisptbackend` | Emisor del token JWT |
| **JwtAudience** | `neorisptbackend` | Audiencia del token JWT |
| **JwtExpirationSeconds** | `3600` | Duración del token (1 hora) |
| **AuthUsername** | `neoris-pt-frontend` | Usuario para autenticación |
| **AuthPassword** | `SecurePassword2026#NeorisSecure` | Contraseña para autenticación |
| **MaxLibros** | `100` | Límite máximo de libros permitidos |
| **ConnectionString** | `Server=localhost,1433;Database=NeorisPTDB;...` | Conexión a SQL Server |

### Puertos y URLs

| Servicio | Puerto | URL |
|----------|--------|-----|
| **Backend API** | 5000 | http://localhost:5000 |
| **Swagger UI** | 5000 | http://localhost:5000/swagger |
| **SQL Server** | 1433 | localhost,1433 |

### Base de Datos

| Propiedad | Valor |
|-----------|-------|
| **Servidor** | localhost,1433 |
| **Base de Datos** | NeorisPTDB |
| **Usuario** | sa |
| **Contraseña** | Neoris2026! |
| **Proveedor** | SQL Server 2022 (Docker) |

## 🛠️ Tecnologías Utilizadas

- **ASP.NET Web API** - Framework web
- **Entity Framework 6.4.4** - ORM
- **Unity Container** - Inyección de dependencias
- **JWT Bearer Authentication** - Autenticación
- **Serilog** - Logging
- **Swashbuckle** - Documentación API (Swagger)
- **SQL Server 2022** - Base de datos
- **Microsoft OWIN** - Middleware de autenticación

## 📚 Recursos y Referencias

- [ASP.NET Web API Documentation](https://docs.microsoft.com/en-us/aspnet/web-api/)
- [Entity Framework 6 Documentation](https://docs.microsoft.com/en-us/ef/ef6/)
- [JWT.io](https://jwt.io/) - Decodificador de JWT
- [Swagger Documentation](https://swagger.io/docs/)
- [Clean Architecture Guide](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

**Última actualización**: Febrero 8, 2026  
**Versión**: 1.0  
**Mantenedor**: Jeyson Andrés García Rodríguez
