using WebAPI;
using Application.Services;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========== Persistencia: EF Core + SQL Server (Entrega 2) ===========
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TPIContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<ICanchaRepository, CanchaRepository>();
builder.Services.AddScoped<ICanchaService, CanchaService>();

var app = builder.Build();

// La base de datos se autogenera si no existe (requisito técnico de la Entrega 2).
// Se hace una sola vez al levantar la app, no en cada request (a diferencia de
// llamarlo desde el constructor del DbContext).
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TPIContext>();
    context.Database.EnsureCreated();
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
