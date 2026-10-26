# TPI_Net
# Sistema de Gestión para un Complejo Deportivo de Fútbol y Pádel

# Integrantes
Lescano Augusto , 54401, augustolescano53@gmail.com

Garibaldi Francisco, 54288, franciscoogaribaldi@gmail.com

## Introducción

Este proyecto consiste en el desarrollo de un sistema de gestión orientado a la administración integral de un complejo deportivo dedicado al alquiler de canchas de fútbol 5 y pádel.

El objetivo principal del sistema es optimizar la gestión de reservas, turnos y usuarios, permitiendo tanto la administración interna del complejo como la interacción directa de los clientes a través de una plataforma digital.

La aplicación contempla dos perfiles de usuario: **Clientes** y **Administradores**. Los clientes podrán registrarse, iniciar sesión, consultar la disponibilidad de las canchas, realizar reservas, alquilar raquetas en caso de asistir a una cancha de padel y acceder al historial de sus actividades. Por su parte, los administradores contarán con herramientas para gestionar usuarios, canchas, turnos, raquetas, reservas y generar reportes de ocupación y recaudación.

El sistema busca mejorar la experiencia de los usuarios, reducir errores operativos y centralizar la información del complejo deportivo en una única plataforma, garantizando un control eficiente de la disponibilidad de las instalaciones y de los ingresos generados.

Para el diseño de la solución se desarrolló un modelo de datos que contempla las principales entidades del dominio, tales como Usuarios, Clientes, Administradores, Canchas, Turnos, Raquetas y Reservas , permitiendo representar las reglas de negocio y asegurar la integridad de la información.

Entre las funcionalidades principales se destacan:

* Registro y autenticación de usuarios.
* Gestión de clientes y administradores.
* Administración de canchas y turnos.
* Creación, consulta y cancelación de reservas.
* Alquiler de raquetas (en caso de necesitarlo).
* Generación de reportes de ocupación y recaudación.
* Control de disponibilidad de canchas según fecha y horario.

Este proyecto fue desarrollado como parte de una actividad académica, aplicando conceptos de análisis, diseño y modelado de bases de datos, así como principios de ingeniería de software orientados a la construcción de sistemas de información.

## Cómo ejecutar el sistema

### Requisitos

* Visual Studio 2022 (o el SDK de .NET 8 para usar la línea de comandos).
* SQL Server: SQL Server Express (`.\SQLEXPRESS`) o LocalDB (`(localdb)\mssqllocaldb`).
* Certificado de desarrollo HTTPS confiado. Si no lo está, ejecutar una vez: `dotnet dev-certs https --trust`.

### Base de datos

* La cadena de conexión está en `WebAPI/appsettings.json`, clave `ConnectionStrings:DefaultConnection`. Hay que ajustarla al servidor SQL de cada máquina, por ejemplo:
  * SQL Server Express: `Server=.\SQLEXPRESS;Database=TPI_CanchaDB;Trusted_Connection=True;TrustServerCertificate=True`
  * LocalDB: `Server=(localdb)\mssqllocaldb;Database=TPI_CanchaDB;Trusted_Connection=True;TrustServerCertificate=True`
* La base `TPI_CanchaDB` se crea sola la primera vez que arranca la WebAPI (`Database.EnsureCreated()`), no hace falta ejecutar ningún script.
* **Importante:** `EnsureCreated()` solo crea las tablas cuando la base no existe; no usa migraciones. Si cambia el modelo (tablas o columnas nuevas) hay que borrar la base para que se vuelva a crear, y se pierden los datos cargados.

### Datos iniciales

Al arrancar la WebAPI, si las tablas están vacías, se cargan automáticamente:

* **Usuario administrador:** usuario `Admin`, contraseña `admin123` (rol Administrador).
* **15 turnos** de una hora, de 08:00 a 23:00.
* **Segundo administrador:** usuario `lgomez`, contraseña `admin123`.
* **Clientes de prueba:** 6 clientes con sus cuentas (`jperez`, `mgarcia`, `lfernandez`, `smartinez`, `drodriguez`, `vlopez`), contraseña `cliente123`.
* **Tipos de cancha / superficies:** Césped sintético, Césped natural, Cemento, Parquet, Vidrio / Blindex y Muro / Cemento.
* **Canchas:** 4 de fútbol y 2 de pádel vinculadas a sus tipos de superficie.
* **Reservas:** ~1160 reservas con líneas de detalle (alquiler de raquetas, pelotas, pecheras, bebidas) y ocupación realista (20-28%) para alimentar los reportes.

### Ejecución

**Desde Visual Studio:** abrir `TPI.sln`. El archivo `TPI.slnLaunch` ya trae configurado el inicio múltiple (WebAPI con el perfil `https`, WindowsForms y Blazor.Server). Elegirlo en el selector de inicio y presionar F5. Si no aparece, se arma desde *Configurar proyectos de inicio* > *Varios proyectos de inicio*.

**Desde la línea de comandos** (cada uno en su propia consola, la WebAPI primero):

```
dotnet run --project WebAPI --launch-profile https
dotnet run --project Blazor.Server --launch-profile https
dotnet run --project WindowsForms
```

### Direcciones y configuración

| Aplicación | Dirección |
|---|---|
| WebAPI | `https://localhost:7111` (Swagger en `https://localhost:7111/swagger`) |
| Web (Blazor Server) | `https://localhost:7253` |
| Escritorio (Windows Forms) | Aplicación de ventanas |

Las dos interfaces leen la dirección de la API de la clave `ApiUrl` de su `appsettings.json` (`WindowsForms/appsettings.json` y `Blazor.Server/appsettings.json`). Si la API corre en otra dirección o puerto, se cambia ahí sin recompilar el código. Si la clave no está, se usa `https://localhost:7111/`.

### Seguridad

* Para entrar al sistema hay que iniciar sesión. La API devuelve un token JWT que las interfaces mandan en cada pedido (`Authorization: Bearer <token>`). El token vence a los 120 minutos (`Jwt:ExpiracionMinutos` en `WebAPI/appsettings.json`).
* Cuando el token vence, el escritorio avisa y vuelve a la pantalla de login; la web muestra el aviso de sesión vencida.
* Autorización por rol: **Administrador** accede a la gestión integral (clientes, canchas, tipos de cancha, turnos, reservas, administradores, usuarios y reportes). **Cliente** accede a "Mis reservas" y "Reservar turno" (la API obtiene el cliente del token, por lo que solo puede consultar, reservar y cancelar sus propias reservas).
* Las contraseñas se guardan hasheadas (SHA-256) y nunca se devuelven en las respuestas de la API.
* Para probar la API desde Swagger: ejecutar `POST /auth/login` con `{"nombreUsuario":"Admin","contrasena":"admin123"}`, copiar el `token` de la respuesta y pegarlo en el botón **Authorize** (sin escribir "Bearer").

### Estructura de la solución

| Proyecto | Responsabilidad |
|---|---|
| `Domain.Model` | Entidades del dominio y sus validaciones |
| `DTOs` | Objetos de transferencia entre la API y las interfaces |
| `Data` | Acceso a datos: Entity Framework Core (`TPIContext`, repositorios) y ADO.NET en la búsqueda de clientes |
| `Application.Services` | Lógica de negocio y reglas (servicios) |
| `WebAPI` | API REST (Minimal APIs), Swagger y autenticación JWT |
| `API.Clients` | Clientes HTTP que usan las dos interfaces para hablar con la API |
| `WindowsForms` | Interfaz de escritorio |
| `Blazor.Server` | Interfaz web |

Las interfaces nunca acceden a la base de datos directamente: `UI → API.Clients → WebAPI → Application.Services → Data → SQL Server`.
