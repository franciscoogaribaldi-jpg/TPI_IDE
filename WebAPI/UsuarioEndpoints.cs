using Application.Services;
using Application.Services.Exceptions;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebAPI
{
    public static class UsuarioEndpoints
    {
        public static void MapUsuarioEndpoints(this WebApplication app)
        {
            app.MapGet("/usuarios/{id}", async (int id, IUsuarioService usuarioService) =>
            {
                UsuarioDTO? dto = await usuarioService.GetAsync(id);
                if (dto == null) return Results.NotFound();
                return Results.Ok(dto);
            })
            .WithName("GetUsuario")
            .Produces<UsuarioDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/usuarios", async (IUsuarioService usuarioService) =>
            {
                var dtos = await usuarioService.GetAllAsync();
                return Results.Ok(dtos);
            })
            .WithName("GetAllUsuarios")
            .Produces<IEnumerable<UsuarioDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/usuarios", async (UsuarioDTO dto, IUsuarioService usuarioService) =>
            {
                try
                {
                    UsuarioDTO creado = await usuarioService.AddAsync(dto);
                    return Results.Created($"/usuarios/{creado.IdUsuario}", creado);
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
            .WithName("AddUsuario")
            .Produces<UsuarioDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/usuarios", async (UsuarioDTO dto, IUsuarioService usuarioService) =>
            {
                try
                {
                    var found = await usuarioService.UpdateAsync(dto);
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
            .WithName("UpdateUsuario")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/usuarios/{id}", async (int id, IUsuarioService usuarioService) =>
            {
                var deleted = await usuarioService.DeleteAsync(id);
                if (!deleted) return Results.NotFound();
                return Results.NoContent();
            })
            .WithName("DeleteUsuario")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}
