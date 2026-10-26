using System;

namespace Domain.Model
{
    public class CanchaFutbol : Cancha
    {
        public TipoFutbol TipoFutbol { get; private set; }

        // Fútbol 5 = 5 por equipo, Fútbol 7 = 7, Fútbol 11 = 11 (el valor del enum es la cantidad por equipo).
        public override int CantidadJugadores => (int)TipoFutbol * 2;

        public override string Caracteristicas => $"Fútbol {(int)TipoFutbol} - {TipoSuperficie?.Nombre ?? "Césped"}";

        // El comando "base" llama al constructor de la clase Padre (Cancha)
        public CanchaFutbol(int idCancha, string nombre, Estado estado, decimal precioPorHora, bool esTechada,
            int idTipoSuperficie, TipoFutbol tipoFutbol)
            : base(idCancha, nombre, estado, precioPorHora, esTechada, idTipoSuperficie)
        {
            SetTipoFutbol(tipoFutbol);
        }

        public void SetTipoFutbol(TipoFutbol tipoFutbol)
        {
            if (!Enum.IsDefined(typeof(TipoFutbol), tipoFutbol))
                throw new ArgumentException("El tipo de cancha de fútbol no es válido.", nameof(tipoFutbol));
            TipoFutbol = tipoFutbol;
        }
    }
}
