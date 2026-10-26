using Application.Services;
using Application.Services.Exceptions;
using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    /// <summary>
    /// Funciones propias del rol Cliente: ver los turnos libres, ver sus reservas, reservar y
    /// cancelar. Quién es el cliente sale siempre del token (claim NameIdentifier = IdUsuario),
    /// nunca del cuerpo del pedido: así nadie puede reservar o cancelar a nombre de otro.
    /// </summary>
    public static class MisReservasEndpoints
    {
        public static void MapMisReservasEndpoints(this WebApplication app)
        {
            // Turnos libres de un día. Lo pueden consultar los dos roles.
            app.MapGet("/turnos-libres", async (DateTime fecha, IReservaService reservaService) =>
            {
                try
                {
                    var libres = await reservaService.GetTurnosLibresAsync(fecha);
                    return Results.Ok(libres);
                }
                catch (ReglaDeNegocioException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetTurnosLibres")
            .Produces<IEnumerable<TurnoLibreDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/mis-reservas", async (IReservaService reservaService, ClaimsPrincipal usuarioLogueado) =>
            {
                try
                {
                    int idUsuario = int.Parse(usuarioLogueado.FindFirstValue(ClaimTypes.NameIdentifier)!);
                    var reservas = await reservaService.GetMisReservasAsync(idUsuario);
                    return Results.Ok(reservas);
                }
                catch (ReglaDeNegocioException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetMisReservas")
            .Produces<IEnumerable<ReservaDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Cliente"));

            app.MapPost("/mis-reservas", async (ReservaDTO dto, IReservaService reservaService, ClaimsPrincipal usuarioLogueado) =>
            {
                try
                {
                    int idUsuario = int.Parse(usuarioLogueado.FindFirstValue(ClaimTypes.NameIdentifier)!);
                    var creada = await reservaService.ReservarComoClienteAsync(dto, idUsuario);
                    return Results.Created($"/mis-reservas/{creada.IdReserva}", creada);
                }
                catch (ReglaDeNegocioException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("ReservarComoCliente")
            .Produces<ReservaDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Cliente"));

            // Cancelar una reserva propia (baja lógica: pasa a Cancelada).
            app.MapDelete("/mis-reservas/{id}", async (int id, IReservaService reservaService, ClaimsPrincipal usuarioLogueado) =>
            {
                try
                {
                    int idUsuario = int.Parse(usuarioLogueado.FindFirstValue(ClaimTypes.NameIdentifier)!);
                    var cancelada = await reservaService.CancelarComoClienteAsync(id, idUsuario);
                    if (!cancelada) return Results.NotFound();
                    return Results.NoContent();
                }
                catch (ReglaDeNegocioException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("CancelarMiReserva")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Cliente"));
        }
    }
}
