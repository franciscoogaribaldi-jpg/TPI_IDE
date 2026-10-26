namespace DTOs
{
    public class TipoSuperficieDTO
    {
        public int IdTipoSuperficie { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Deporte { get; set; } = string.Empty; // "Futbol" o "Padel"
        public int Estado { get; set; } = 1; // 1 = Activo, 0 = Inactivo
    }
}
