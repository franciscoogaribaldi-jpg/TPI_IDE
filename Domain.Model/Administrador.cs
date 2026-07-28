using System;

namespace Domain.Model
{
    public class Administrador
    {
        public int IdAdministrador { get; private set; }

        public int IdUsuario { get; private set; }
        public Usuario? Usuario { get; private set; }

        public string NombreCompleto { get; private set; }
        public string Telefono { get; private set; }
        public Estado Estado { get; private set; }

        public Administrador(int idAdministrador, int idUsuario, string nombreCompleto, string telefono, Estado estado)
        {
            SetIdAdministrador(idAdministrador);
            SetIdUsuario(idUsuario);
            SetNombreCompleto(nombreCompleto);
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

        public void SetNombreCompleto(string nombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombreCompleto));
            NombreCompleto = nombreCompleto;
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