using Application.Services;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebAPI
{
    public static class AuthEndpoints
    {
        // Entrega 3: el login ahora devuelve un token JWT, que el cliente (WinForms o
        // Blazor) tiene que mandar en cada pedido siguiente (header Authorization: Bearer).
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/login", async (LoginRequestDTO request, IUsuarioService usuarioService, JwtTokenGenerator tokenGenerator) =>
            {
                LoginResponseDTO? respuesta = await usuarioService.LoginAsync(request);
                if (respuesta == null) return Results.Unauthorized();

                respuesta.Token = tokenGenerator.GenerarToken(respuesta.IdUsuario, respuesta.NombreUsuario, respuesta.Rol);
                return Results.Ok(respuesta);
            })
            .WithName("Login")
            .Produces<LoginResponseDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithOpenApi();
        }
    }
}
