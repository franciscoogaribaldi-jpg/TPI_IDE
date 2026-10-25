using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public enum Estado
    {
        Activo = 1,
        Inactivo = 0
    }

    public enum RolUsuario
    {
        Administrador = 1,
        Cliente = 2
    }

    public enum EstadoReserva
    {
        Pendiente = 1,
        Confirmada = 2,
        Cancelada = 3,
        Finalizada = 4
    }

    public enum TipoCancha
    {
        Futbol = 1,
        Padel = 2
    }

    // El valor es la cantidad de jugadores por equipo (CanchaFutbol lo usa para calcular el total).
    public enum TipoFutbol
    {
        Futbol5 = 5,
        Futbol7 = 7,
        Futbol11 = 11
    }

    public enum TipoSuperficie
    {
        CespedSintetico = 1,
        CespedNatural = 2,
        Cemento = 3
    }

    public enum TipoPared
    {
        Vidrio = 1,
        Muro = 2
    }
}