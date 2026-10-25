using API.Clients;
using Blazor.Server.Components;
using Blazor.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<SesionUsuario>();

// URL de la WebAPI desde appsettings.json ("ApiUrl"). Es la misma para todos los usuarios,
// por eso alcanza con la propiedad estática (el token, en cambio, va por sesión).
BaseApiClient.BaseUrl = builder.Configuration["ApiUrl"] ?? BaseApiClient.BaseUrlPorDefecto;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
