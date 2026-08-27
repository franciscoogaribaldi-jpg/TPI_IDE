namespace Application.Services.Exceptions
{
    /// <summary>
    /// Se lanza cuando una operación viola una regla de negocio (ej: DNI duplicado,
    /// usuario ya registrado). No se usa para datos inválidos en sí mismos: para eso
    /// están los ArgumentException que ya tira el Modelo de Dominio en sus setters.
    /// Separarla de Exception genérica permite a los endpoints (y a futuro, a los
    /// formularios de escritorio) capturarla puntualmente y mostrar un mensaje
    /// amigable, sin ocultar errores no controlados detrás del mismo catch.
    /// </summary>
    public class ReglaDeNegocioException : Exception
    {
        public ReglaDeNegocioException(string message) : base(message)
        {
        }
    }
}
