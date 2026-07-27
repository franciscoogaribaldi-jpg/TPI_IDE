using System;

namespace DTOs
{
    public class ClienteDTO
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }

        // Usamos int para que en Swagger se pueda mandar un 1 (Activo) o 0 (Inactivo)
        public int Estado { get; set; }
    }
}