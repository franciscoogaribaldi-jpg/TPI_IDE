using Application.Services;
using Application.Services.Exceptions;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebAPI
{
    public static class TurnoEndpoints
    {
        public static void MapTurnoEndpoints(this WebApplication app)
        {
            app.MapGet("/turnos/{id}", async (int id, ITurnoService turnoService) =>
            {
                TurnoDTO? dto = await turnoService.GetAsync(id);
                if (dto == null) return Results.NotFound();
                return Results.Ok(dto);
            })
            .WithName("GetTurno")
            .Produces<TurnoDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/turnos", async (ITurnoService turnoService) =>
            {
                var dtos = await turnoService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllTurnos")
            .Produces<IEnumerable<TurnoDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapPost("/turnos", async (TurnoDTO dto, ITurnoService turnoService) =>
            {
                try
                {
                    TurnoDTO creado = await turnoService.AddAsync(dto);
                    return Results.Created($"/turnos/{creado.IdTurno}", creado);
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
            .WithName("AddTurno")
            .Produces<TurnoDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapPut("/turnos", async (TurnoDTO dto, ITurnoService turnoService) =>
            {
                try
                {
                    var found = await turnoService.UpdateAsync(dto);
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
            .WithName("UpdateTurno")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapDelete("/turnos/{id}", async (int id, ITurnoService turnoService) =>
            {
                var deleted = await turnoService.DeleteAsync(id);
                if (!deleted) return Results.NotFound();
                return Results.NoContent();
            })
            .WithName("DeleteTurno")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}