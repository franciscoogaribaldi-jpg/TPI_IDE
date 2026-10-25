using System;

namespace Domain.Model
{
    public class CanchaPadel : Cancha
    {
        public TipoPared TipoPared { get; private set; }

        // El pádel siempre se juega de a 4 (dos parejas), sin importar la cancha.
        public override int CantidadJugadores => 4;

        public override string Caracteristicas => TipoPared == TipoPared.Vidrio ? "Paredes de vidrio" : "Paredes de muro";

        public CanchaPadel(int idCancha, string nombre, Estado estado, decimal precioPorHora, bool esTechada, TipoPared tipoPared)
            : base(idCancha, nombre, estado, precioPorHora, esTechada)
        {
            SetTipoPared(tipoPared);
        }

        public void SetTipoPared(TipoPared tipoPared)
        {
            if (!Enum.IsDefined(typeof(TipoPared), tipoPared))
                throw new ArgumentException("El tipo de pared no es válido.", nameof(tipoPared));
            TipoPared = tipoPared;
        }
    }
}
