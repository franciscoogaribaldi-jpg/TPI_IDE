namespace DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;

        // Solo se usa como dato de ENTRADA (alta). El servicio nunca lo devuelve relleno.
        public string Contrasena { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public int Rol { get; set; }
        public int Estado { get; set; }
    }
}
