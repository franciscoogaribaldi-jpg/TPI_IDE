using DTOs;

namespace WindowsForms
{
    /// <summary>
    /// Usuario logueado, en memoria mientras dura el proceso. Deliberadamente simple
    /// para la Entrega 2 (sin tokens: eso es requisito recién de la Entrega 3). Si más
    /// adelante Blazor necesita algo parecido, esto se puede extraer a una interfaz
    /// compartida sin romper nada acá — no lo hacemos ahora porque todavía no hace falta.
    /// </summary>
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
        }
    }
}
