using WebAPI;
using Application.Services;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Domain.Model;


var builder = WebApplication.CreateBuilder(args); // instanciamos el builder

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========== Persistencia: EF Core + SQL Server (Entrega 2) ===========
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // pedimos el connectionString para conectar a la bd
builder.Services.AddDbContext<TPIContext>(options => options.UseSqlServer(connectionString)); // conectamos SqlServer con el conectionString

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); //crea un respositorio nuevo por cada usuario de internet que haga clic.
builder.Services.AddScoped<IUsuarioService, UsuarioService>(); //crea un Servicio nuevo por cada usuario de internet que haga clic.

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<ICanchaRepository, CanchaRepository>();
builder.Services.AddScoped<ICanchaService, CanchaService>();

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

app.MapAuthEndpoints();
app.MapUsuarioEndpoints();
app.MapClienteEndpoints();
app.MapCanchaEndpoints();

app.Run();
