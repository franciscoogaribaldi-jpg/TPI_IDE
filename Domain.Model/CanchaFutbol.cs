namespace Domain.Model
{
    public class CanchaFutbol : Cancha
    {
  

        public CanchaFutbol(int idCancha, string nombre, Estado estado, decimal precioPorHora)
            : base(idCancha, nombre, estado, precioPorHora)
        {
        }
    }
}