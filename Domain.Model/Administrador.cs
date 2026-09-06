using System;

namespace Domain.Model
{
    public class Administrador
    {
        public int IdAdministrador { get; private set; }

        public int IdUsuario { get; private set; }
        public Usuario? Usuario { get; private set; }

        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Telefono { get; private set; }
        public Estado Estado { get; private set; }

        public Administrador(int idAdministrador, int idUsuario, string nombre, string apellido, string telefono, Estado estado)
        {
            SetIdAdministrador(idAdministrador);
            SetIdUsuario(idUsuario);
            SetNombre(nombre);
            SetApellido(apellido);
            SetTelefono(telefono);
            SetEstado(estado);
        }

        public void SetIdAdministrador(int idAdministrador)
        {
            if (idAdministrador < 0)
                throw new ArgumentException("El Id no puede ser negativo.", nameof(idAdministrador));
            IdAdministrador = idAdministrador;
        }

        public void SetIdUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("El Id de usuario debe ser mayor a 0.", nameof(idUsuario));
            IdUsuario = idUsuario;
        }

        public void SetUsuario(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);
            Usuario = usuario;
            IdUsuario = usuario.IdUsuario;
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
            Nombre = nombre;
        }

        public void SetApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido no puede estar vacío.", nameof(apellido));
            Apellido = apellido;
        }

        public void SetTelefono(string telefono)
        {
            Telefono = telefono;
        }

        public void SetEstado(Estado estado)
        {
            Estado = estado;
        }
    }
}