using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class ReporteEndpoints
    {
        // Los reportes son información de gestión: solo para el Administrador.
        public static void MapReporteEndpoints(this WebApplication app)
        {
            app.MapGet("/reportes/ocupacion", async (DateTime desde, DateTime hasta, IReporteService reporteService) =>
            {
                try
                {
                    var datos = await reporteService.GetOcupacionAsync(desde, hasta);
                    return Results.Ok(datos);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetReporteOcupacion")
            .Produces<IEnumerable<ReporteOcupacionDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));

            app.MapGet("/reportes/recaudacion", async (DateTime desde, DateTime hasta, IReporteService reporteService) =>
            {
                try
                {
                    var datos = await reporteService.GetRecaudacionAsync(desde, hasta);
                    return Results.Ok(datos);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetReporteRecaudacion")
            .Produces<IEnumerable<ReporteRecaudacionDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization(policy => policy.RequireRole("Administrador"));
        }
    }
}
