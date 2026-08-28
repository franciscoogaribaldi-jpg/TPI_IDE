namespace Domain.Model
{
    public class ClienteCriteria
    {
        public string Texto { get; private set; }

        public ClienteCriteria(string? texto)
        {
            // Sin filtro (texto=null, ej. no mandaron el query param) = traer todos los
            // clientes, no un error. Antes esto explotaba con NullReferenceException.
            Texto = (texto ?? string.Empty).Trim();
        }
    }
}