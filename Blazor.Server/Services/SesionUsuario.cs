using API.Clients;
using DTOs;

namespace Blazor.Server.Services
{
    // Equivalente web de WindowsForms/SesionActual.cs.
    // Se registra como Scoped: en Blazor Server es UNA instancia por usuario conectado,
    // así cada navegador tiene su propia sesión y su propio token.
    public class SesionUsuario
    {
        public LoginResponseDTO? Usuario { get; private set; }

        public bool EstaLogueado => Usuario != null;

        // 1 = Administrador, 2 = Cliente (Domain.Model.RolUsuario).
        // Igual que en Home.cs del WinForms, no referenciamos Domain.Model desde la UI.
        public bool EsAdministrador => Usuario?.Rol == 1;
        public string? Token => Usuario?.Token;

        // Aviso para que el menú y el layout se redibujen al entrar o salir.
        public event Action? OnCambio;

        public async Task<bool> IniciarSesionAsync(string nombreUsuario, string contrasena)
        {
            var respuesta = await AuthApiClient.LoginAsync(nombreUsuario, contrasena);
            if (respuesta == null) return false; // credenciales incorrectas

            Usuario = respuesta;
            OnCambio?.Invoke();
            return true;
        }

        public void CerrarSesion()
        {
            Usuario = null;
            OnCambio?.Invoke();
        }
    }
}