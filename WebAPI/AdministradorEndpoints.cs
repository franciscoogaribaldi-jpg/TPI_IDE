using Application.Services;
using Application.Services.Exceptions;
using DTOs;
using System.Security.Claims;

namespace WebAPI
{
    public static class AdministradorEndpoints
    {
        public static void MapAdministradorEndpoints(this WebApplication app)
        {
            app.MapGet("/administradores/{id}", async (int id, IAdministradorService administradorService) =>
            {
                AdministradorDTO? dto = await administradorService.GetAsync(id);
                if (dto == null) return Results.NotFound();
                return Results.Ok(dto);
            })
            .WithName("GetAdministrador")
            .Produces<AdministradorDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapGet("/administradores", async (IAdministradorService administradorService) =>
            {
                var dtos = await administradorService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllAdministradores")
            .Produces<IEnumerable<AdministradorDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapPost("/administradores", async (AdministradorDTO dto, IAdministradorService administradorService) =>
            {
                try
                {
                    AdministradorDTO creado = await administradorService.AddAsync(dto);
                    return Results.Created($"/administradores/{creado.IdAdministrador}", creado);
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
            .WithName("AddAdministrador")
            .Produces<AdministradorDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapPut("/administradores", async (AdministradorDTO dto, IAdministradorService administradorService) =>
            {
                try
                {
                    var found = await administradorService.UpdateAsync(dto);
                    if (!found) return Results.NotFound();
                    return Results.NoContent();
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
            .WithName("UpdateAdministrador")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapDelete("/administradores/{id}", async (int id, IAdministradorService administradorService, ClaimsPrincipal usuarioLogueado) =>
            {
                try
                {
                    // Baja lógica. Quién está logueado sale del token, para no dejar que se elimine a sí mismo.
                    int? idUsuario = int.TryParse(usuarioLogueado.FindFirstValue(ClaimTypes.NameIdentifier), out var idLogueado) ? idLogueado : null;
                    var deleted = await administradorService.DeleteAsync(id, idUsuario);
                    if (!deleted) return Results.NotFound();
                    return Results.NoContent();
                }
                catch (ReglaDeNegocioException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("DeleteAdministrador")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}
