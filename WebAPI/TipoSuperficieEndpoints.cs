using Application.Services;
using Application.Services.Exceptions;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebAPI
{
    public static class TipoSuperficieEndpoints
    {
        public static void MapTipoSuperficieEndpoints(this WebApplication app)
        {
            app.MapGet("/tipos-superficie/{id}", async (int id, ITipoSuperficieService service) =>
            {
                var dto = await service.GetAsync(id);
                if (dto == null) return Results.NotFound();
                return Results.Ok(dto);
            })
            .WithName("GetTipoSuperficie")
            .Produces<TipoSuperficieDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapGet("/tipos-superficie", async (ITipoSuperficieService service) =>
            {
                var dtos = await service.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllTiposSuperficie")
            .Produces<IEnumerable<TipoSuperficieDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapGet("/tipos-superficie/deporte/{deporte}", async (string deporte, ITipoSuperficieService service) =>
            {
                var dtos = await service.GetByDeporteAsync(deporte);
                return Results.Ok(dtos);
            })
            .WithName("GetTiposSuperficieByDeporte")
            .Produces<IEnumerable<TipoSuperficieDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapPost("/tipos-superficie", async (TipoSuperficieDTO dto, ITipoSuperficieService service) =>
            {
                try
                {
                    var creado = await service.AddAsync(dto);
                    return Results.Created($"/tipos-superficie/{creado.IdTipoSuperficie}", creado);
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
            .WithName("AddTipoSuperficie")
            .Produces<TipoSuperficieDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapPut("/tipos-superficie", async (TipoSuperficieDTO dto, ITipoSuperficieService service) =>
            {
                try
                {
                    var found = await service.UpdateAsync(dto);
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
            .WithName("UpdateTipoSuperficie")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapDelete("/tipos-superficie/{id}", async (int id, ITipoSuperficieService service) =>
            {
                try
                {
                    var deleted = await service.DeleteAsync(id);
                    if (!deleted) return Results.NotFound();
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
            .WithName("DeleteTipoSuperficie")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}
