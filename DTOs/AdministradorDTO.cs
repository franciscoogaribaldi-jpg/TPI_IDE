using System.Text.Json.Serialization;

namespace DTOs
{
    public class AdministradorDTO
    {
        public int IdAdministrador { get; set; }
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int Estado { get; set; }

        // Datos del usuario de acceso (rol Administrador). Se devuelven siempre; en el ALTA se
        // reciben para crear el usuario junto con el administrador, igual que en ClienteDTO.
        public string? NombreUsuario { get; set; }
        public string? Email { get; set; }

        // Solo de ENTRADA, y solo en el alta. El servidor nunca la devuelve.
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Contrasena { get; set; }

        // Solo de SALIDA, en el alta: true si ese nombre de usuario era de un administrador
        // eliminado y se lo recuperó en lugar de crear uno nuevo.
        public bool Recuperado { get; set; }
    }
}
