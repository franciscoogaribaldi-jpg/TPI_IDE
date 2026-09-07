using Application.Services;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebAPI
{
    public static class AuthEndpoints
    {
        // Login/logout simple para la Entrega 2 (sin tokens: eso es requisito recién
        // de la Entrega 3). El WinForms guarda el LoginResponseDTO en memria mientras
        // dura la sesión y lo descarta al hacer logout.
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/login", async (LoginRequestDTO request, IUsuarioService usuarioService) =>
            {
                LoginResponseDTO? respuesta = await usuarioService.LoginAsync(request);
                if (respuesta == null) return Results.Unauthorized();
                return Results.Ok(respuesta);
            })
            .WithName("Login")
            .Produces<LoginResponseDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi();
        }
    }
}
