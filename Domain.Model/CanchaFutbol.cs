namespace Domain.Model
{
    public class CanchaFutbol : Cancha
    {
        // El comando "base" llama al constructor de la clase Padre (Cancha)
        public CanchaFutbol(int idCancha, string nombre, Estado estado, decimal precioPorHora)
            : base(idCancha, nombre, estado, precioPorHora)
        {
        }
    }
}