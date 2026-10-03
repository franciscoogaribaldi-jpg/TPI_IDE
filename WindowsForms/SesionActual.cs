using DTOs;
using API.Clients;

namespace WindowsForms
{

    internal static class SesionActual
    {
        public static LoginResponseDTO? Usuario { get; private set; }

        public static bool EstaLogueado => Usuario != null;

        public static void IniciarSesion(LoginResponseDTO usuario)
        {
            Usuario = usuario;
        }

        public static void CerrarSesion()
        {
            Usuario = null;
            BaseApiClient.TokenActual = null;
        }
    }
}
