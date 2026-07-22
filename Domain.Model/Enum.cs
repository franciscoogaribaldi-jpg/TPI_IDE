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
}