using WebAPI;
using Application.Services;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Domain.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args); // instanciamos el builder

builder.Services.AddEndpointsApiExplorer();

// Configurar JWT Authentication en Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Pegá acá el token (sin la palabra 'Bearer', Swagger la agrega sola)."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =========== Persistencia: EF Core + SQL Server (Entrega 2) ===========
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // pedimos el connectionString para conectar a la bd
builder.Services.AddDbContext<TPIContext>(options => options.UseSqlServer(connectionString)); // conectamos SqlServer con el conectionString

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); //crea un respositorio nuevo por cada usuario de internet que haga clic.
builder.Services.AddScoped<IUsuarioService, UsuarioService>(); //crea un Servicio nuevo por cada usuario de internet que haga clic.

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<ICanchaRepository, CanchaRepository>();
builder.Services.AddScoped<ICanchaService, CanchaService>();

builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
builder.Services.AddScoped<ITurnoService, TurnoService>();

builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IReservaService, ReservaService>();

builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();
builder.Services.AddScoped<IAdministradorService, AdministradorService>();

builder.Services.AddScoped<IReporteService, ReporteService>();

// =========== Seguridad: autenticación con JWT (Entrega 3) ===========
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // a partir de ahora puede autenticar peticiones. esquema de seguridad bearer
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters // lista de requisitos que el token tiene que cumplir para que lo dejen pasar
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            // Por defecto .NET da 5 minutos de tolerancia después del vencimiento. En cero, el
            // token vence en el momento exacto (sirve para probar la vuelta al login).
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(); // esto es para qeu el builder autorice o no a un usuario, si tiene token lo autoriza, sino no.
builder.Services.AddScoped<JwtTokenGenerator>();

var app = builder.Build(); // construimos el servidor con todo el builder modificado

// La base de datos se autogenera si no existe (requisito técnico de la Entrega 2).
// Se hace una sola vez al levantar la app, no en cada request (a diferencia de llamarlo desde el constructor del DbContext).
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TPIContext>();
    context.Database.EnsureCreated();

    // creamos el usuario de inicio para no tener que hacerlo desde swagger

    if (!context.Usuarios.Any())
    {
        var hash = PasswordHasher.Hash("admin123");

        var usuarioAdmin = new Usuario(
            idUsuario: 0,
            nombreUsuario: "Admin",
            contrasena: hash,
            email: "admin@gmail.com",
            rol: RolUsuario.Administrador,
            estado: Estado.Activo
            );

        context.Usuarios.Add(usuarioAdmin);
        context.SaveChanges();
    }

    // creamos unos turnos pre hechos en la base de datos para poder realizar altas de reservas
    if (!context.Turnos.Any())
    {

        for (int hora = 8; hora < 23; hora++)
        {
            context.Turnos.Add(new Turno(
                idTurno: 0,
                horaInicio: new TimeSpan(hora, 0, 0),
                horaFin: new TimeSpan(hora + 1, 0, 0),
                estado: Estado.Activo));
        }
        context.SaveChanges();
    }

    // Administradores, clientes, canchas y reservas de ejemplo (ver DatosDePrueba.cs).
    DatosDePrueba.Cargar(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapUsuarioEndpoints();
app.MapClienteEndpoints();
app.MapCanchaEndpoints();
app.MapTurnoEndpoints();
app.MapReservaEndpoints();
app.MapAdministradorEndpoints();
app.MapReporteEndpoints();

app.Run();
