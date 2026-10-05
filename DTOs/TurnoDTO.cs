using System;


namespace DTOs
{
    public class TurnoDTO
    {
        public int IdTurno { get; set; }
        public string HoraInicio { get; set; } = string.Empty;
        public string HoraFin { get; set; } = string.Empty;

        public int Estado { get; set; }
    }
}
