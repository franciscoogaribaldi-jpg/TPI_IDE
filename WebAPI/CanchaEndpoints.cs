using Application.Services;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebAPI
{
    public static class CanchaEndpoints
    {
        public static void MapCanchaEndpoints(this WebApplication app)
        {
            app.MapGet("/canchas/{id}", async (int id, ICanchaService canchaService) =>
            {
                CanchaDTO? dto = await canchaService.GetAsync(id);
                if (dto == null) return Results.NotFound();
                return Results.Ok(dto);
            })
            .WithName("GetCancha")
            .Produces<CanchaDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/canchas", async (ICanchaService canchaService) =>
            {
                var dtos = await canchaService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllCanchas")
            .Produces<IEnumerable<CanchaDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/canchas", async (CanchaDTO dto, ICanchaService canchaService) =>
            {
                try
                {
                    CanchaDTO canchaDTO = await canchaService.AddAsync(dto);
                    return Results.Created($"/canchas/{canchaDTO.IdCancha}", canchaDTO);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddCancha")
            .Produces<CanchaDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/canchas", async (CanchaDTO dto, ICanchaService canchaService) =>
            {
                try
                {
                    var found = await canchaService.UpdateAsync(dto);
                    if (!found) return Results.NotFound();
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateCancha")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/canchas/{id}", async (int id, ICanchaService canchaService) =>
            {
                var deleted = await canchaService.DeleteAsync(id);
                if (!deleted) return Results.NotFound();
                return Results.NoContent();
            })
            .WithName("DeleteCancha")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}