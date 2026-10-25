using System;

namespace Domain.Model
{
    public class CanchaFutbol : Cancha
    {
        public TipoFutbol TipoFutbol { get; private set; }
        public TipoSuperficie TipoSuperficie { get; private set; }

        // Fútbol 5 = 5 por equipo, Fútbol 7 = 7, Fútbol 11 = 11 (el valor del enum es la cantidad por equipo).
        public override int CantidadJugadores => (int)TipoFutbol * 2;

        public override string Caracteristicas => $"Fútbol {(int)TipoFutbol} - {DescribirSuperficie()}";

        // El comando "base" llama al constructor de la clase Padre (Cancha)
        public CanchaFutbol(int idCancha, string nombre, Estado estado, decimal precioPorHora, bool esTechada,
            TipoFutbol tipoFutbol, TipoSuperficie tipoSuperficie)
            : base(idCancha, nombre, estado, precioPorHora, esTechada)
        {
            SetTipoFutbol(tipoFutbol);
            SetTipoSuperficie(tipoSuperficie);
        }

        public void SetTipoFutbol(TipoFutbol tipoFutbol)
        {
            if (!Enum.IsDefined(typeof(TipoFutbol), tipoFutbol))
                throw new ArgumentException("El tipo de cancha de fútbol no es válido.", nameof(tipoFutbol));
            TipoFutbol = tipoFutbol;
        }

        public void SetTipoSuperficie(TipoSuperficie tipoSuperficie)
        {
            if (!Enum.IsDefined(typeof(TipoSuperficie), tipoSuperficie))
                throw new ArgumentException("El tipo de superficie no es válido.", nameof(tipoSuperficie));
            TipoSuperficie = tipoSuperficie;
        }

        private string DescribirSuperficie() => TipoSuperficie switch
        {
            TipoSuperficie.CespedSintetico => "Césped sintético",
            TipoSuperficie.CespedNatural => "Césped natural",
            TipoSuperficie.Cemento => "Cemento",
            _ => TipoSuperficie.ToString()
        };
    }
}
