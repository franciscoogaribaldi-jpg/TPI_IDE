namespace API.Clients
{
    /// <summary>
    /// Se lanza cuando la API responde 401 a un pedido que SÍ llevaba token: el token venció
    /// (o dejó de ser válido) y hay que volver a iniciar sesión. Cada interfaz decide qué hacer:
    /// el escritorio vuelve al login (ErroresUI) y la web muestra el mensaje.
    /// </summary>
    public class SesionExpiradaException : Exception
    {
        public SesionExpiradaException()
            : base("Tu sesión venció. Volvé a iniciar sesión.")
        {
        }
    }
}
