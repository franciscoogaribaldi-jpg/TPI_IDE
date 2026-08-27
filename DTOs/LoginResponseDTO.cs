namespace DTOs
{
    public class LoginResponseDTO
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;

        // int para no acoplar DTOs a Domain.Model; el WinForms lo castea a RolUsuario.
        public int Rol { get; set; }
    }
}
