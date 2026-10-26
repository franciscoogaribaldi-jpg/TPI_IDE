using System;

namespace Domain.Model
{
    public class CanchaPadel : Cancha
    {
        // El pádel siempre se juega de a 4 (dos parejas), sin importar la cancha.
        public override int CantidadJugadores => 4;

        public override string Caracteristicas => $"Pádel - {TipoSuperficie?.Nombre ?? "Pared/Superficie"}";

        public CanchaPadel(int idCancha, string nombre, Estado estado, decimal precioPorHora, bool esTechada, int idTipoSuperficie)
            : base(idCancha, nombre, estado, precioPorHora, esTechada, idTipoSuperficie)
        {
        }
    }
}
