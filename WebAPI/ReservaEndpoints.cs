using Application.Services;
using Application.Services.Exceptions;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebAPI
{
    public static class ReservaEndpoints
    {
        public static void MapReservaEndpoints(this WebApplication app)
        {
            app.MapGet("/reservas/{id}", async (int id, IReservaService reservaService) =>
            {
                ReservaDTO? dto = await reservaService.GetAsync(id);
                if (dto == null) return Results.NotFound();
                return Results.Ok(dto);
            })
            .WithName("GetReserva")
            .Produces<ReservaDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapGet("/reservas", async (IReservaService reservaService) =>
            {
                var dtos = await reservaService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllReservas")
            .Produces<IEnumerable<ReservaDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization();

            app.MapPost("/reservas", async (ReservaDTO dto, IReservaService reservaService) =>
            {
                try
                {
                    ReservaDTO creada = await reservaService.AddAsync(dto);
                    return Results.Created($"/reservas/{creada.IdReserva}", creada);
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
            .WithName("AddReserva")
            .Produces<ReservaDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapPut("/reservas", async (ReservaDTO dto, IReservaService reservaService) =>
            {
                try
                {
                    var found = await reservaService.UpdateAsync(dto);
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
            .WithName("UpdateReserva")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapDelete("/reservas/{id}", async (int id, IReservaService reservaService) =>
            {
                var deleted = await reservaService.DeleteAsync(id);
                if (!deleted) return Results.NotFound();
                return Results.NoContent();
            })
            .WithName("DeleteReserva")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}