# Neoris Backend

API REST desarrollada con ASP.NET Web API (.NET Framework 4.8) que expone endpoints para gestionar autores. Implementado siguiendo principios de **Clean Architecture** y **SOLID**.

## 🚀 Características

- **ASP.NET Web API** (.NET Framework 4.8)
- **RESTful API** con controladores
- **Swagger** para documentación interactiva
- **CORS** habilitado
- **JWT** para autenticación
- Operaciones **CRUD completas**
- Compatible con Windows Server / IIS
- ✅ **Arquitectura Limpia (Clean Architecture)**
- ✅ **Separación de Responsabilidades**
- ✅ **Inyección de Dependencias (Unity)**
- ✅ **Repository Pattern y Unit of Work**
- ✅ **DTOs para contratos de API**
- ✅ **Principios SOLID implementados**
- ✅ **Fácil de testear y mantener**

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

## 🏃 Ejecución

### Desde Visual Studio:
1. Abre el proyecto en Visual Studio
2. Presiona F5 para ejecutar con debugging
3. La aplicación se abrirá en IIS Express

### Compilar desde línea de comandos:
```bash
# Restaurar paquetes NuGet
nuget restore

# Compilar el proyecto
msbuild neoris-pt-backend.csproj /p:Configuration=Release
```

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

### Opción 3: Docker / Docker Compose

#### Requisitos previos:
- Docker instalado
- Docker Compose instalado (viene con Docker Desktop)

#### Compilar imagen Docker:
```bash
# Desde la raíz del proyecto (donde está el Dockerfile)
docker build -t neoris-pt-backend:latest .

# O si necesitas especificar la versión de .NET Framework
docker build -t neoris-pt-backend:4.8 \
  --build-arg DOTNET_VERSION=4.8 .
```

#### Ejecutar con Docker:
```bash
# Ejecutar el contenedor
docker run -d \
  --name neoris-backend \
  -p 5000:80 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  neoris-pt-backend:latest

# Acceder a la aplicación
# http://localhost:5000/swagger
```

#### Ejecutar con Docker Compose:
En el archivo `docker-compose.yml` ya está configurado el backend. Simplemente ejecuta:

```bash
# Desde el directorio raíz (donde está docker-compose.yml)
docker-compose up -d

# Ver los logs
docker-compose logs -f neoris-pt-backend

# Detener los servicios
docker-compose down

# Detener sin eliminar volúmenes
docker-compose stop
```

#### Variables de entorno en Docker:
```yaml
# En docker-compose.yml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - DATABASE_CONNECTION_STRING=Server=sqlserver;Database=NeorisPT;...
```

#### Ver logs del contenedor:
```bash
# Ver logs en tiempo real
docker logs -f neoris-backend

# O con Docker Compose
docker-compose logs -f neoris-pt-backend
```

#### Limpiar contenedores y imágenes:
```bash
# Eliminar contenedor
docker rm -f neoris-backend

# Eliminar imagen
docker rmi neoris-pt-backend:latest

# Con Docker Compose
docker-compose down -v  # Elimina volúmenes también
```

---

### Ejecutar con IIS Express

```cmd
"C:\Program Files (x86)\IIS Express\iisexpress.exe" /path:C:\Users\jeoga\Documents\Neoris\neoris-pt-backend /port:5000
```

### Verificar instalación

```powershell
reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v Version
```

```cmd
where msbuild
where nuget
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
    "username": "admin",
    "password": "admin123"
  }'
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "mensaje": "Login exitoso"
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
    "email": "gabriel.garcia@neoris.com"
  },
  {
    "id": 2,
    "nombre": "Pablo Neruda",
    "fechaNacimiento": "1904-07-12",
    "ciudadProcedencia": "Parral",
    "email": "pablo.neruda@neoris.com"
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
  "email": "gabriel.garcia@neoris.com"
}
```

### 4️⃣ Crear un autor (POST)

**Request:**
```bash
curl -X POST http://localhost:5000/api/v1/autores \
  -H "Content-Type: application/json" \
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
