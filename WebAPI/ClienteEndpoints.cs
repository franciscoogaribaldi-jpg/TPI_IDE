using Application.Services;
using Application.Services.Exceptions;
using DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WebAPI
{
    public static class ClienteEndpoints
    {
        public static void MapClienteEndpoints(this WebApplication app)
        {
            app.MapGet("/clientes/buscar", async (string? texto, IClienteService clienteService) =>
            {
                var criteria = new ClienteCriteriaDTO { Texto = texto };
                var clientes = await clienteService.GetByCriteriaAsync(criteria);
                return Results.Ok(clientes);
            })
            .WithName("GetClientesByCriteria")
            .Produces<IEnumerable<ClienteDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapGet("/clientes/{id}", async (int id, IClienteService clienteService) =>
            {
                ClienteDTO? dto = await clienteService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetCliente")
            .Produces<ClienteDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapGet("/clientes", async (IClienteService clienteService) =>
            {
                var dtos = await clienteService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllClientes")
            .Produces<IEnumerable<ClienteDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador")); 

            app.MapPost("/clientes", async (ClienteDTO dto, IClienteService clienteService) =>
            {
                try
                {
                    ClienteDTO clienteDTO = await clienteService.AddAsync(dto); // devuelve el dto pero con los datos que no se autocompletan (id lo incremente sqlServer)
                    return Results.Created($"/clientes/{clienteDTO.IdCliente}", clienteDTO); // produce la respuesta de que se creo (201)
                }
                catch (ReglaDeNegocioException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    // Datos inválidos detectados por el Modelo de Dominio (setters de Cliente/Usuario)
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddCliente")
            .Produces<ClienteDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapPut("/clientes", async (ClienteDTO dto, IClienteService clienteService) =>
            {
                try
                {
                    var found = await clienteService.UpdateAsync(dto);
                    if (!found) return Results.NotFound();

                    return Results.NoContent(); // este es el exito porqque al actualizar no devuelve nada
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
            .WithName("UpdateCliente")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapDelete("/clientes/{id}", async (int id, IClienteService clienteService) =>
            {
                var deleted = await clienteService.DeleteAsync(id);
                if (!deleted) return Results.NotFound();

                return Results.NoContent();
            })
            .WithName("DeleteCliente")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}
