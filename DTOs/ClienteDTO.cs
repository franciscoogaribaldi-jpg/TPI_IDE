using System;
using System.Text.Json.Serialization;

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
        public int Estado { get; set; }

        // Datos del usuario de acceso del cliente. Se devuelven siempre; en el ALTA se reciben
        // para crear el usuario (rol Cliente) junto con el cliente.
        public string? NombreUsuario { get; set; }
        public string? Email { get; set; }

        // Solo de ENTRADA, y solo en el alta. El servidor nunca la devuelve.
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Contrasena { get; set; }
    }
}
