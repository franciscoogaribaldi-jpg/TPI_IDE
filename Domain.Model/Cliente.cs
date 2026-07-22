using System;

namespace Domain.Model
{
    public class Cliente
    {
        public int IdCliente { get; private set; }

        // Relación con el Usuario
        public int IdUsuario { get; private set; }
        public Usuario? Usuario { get; private set; }

        public string NombreCompleto { get; private set; }
        public string Dni { get; private set; }
        public string Telefono { get; private set; }
        public DateTime FechaNacimiento { get; private set; }
        public Estado Estado { get; private set; }

        public Cliente(int idCliente, int idUsuario, string nombreCompleto, string dni, string telefono, DateTime fechaNacimiento, Estado estado)
        {
            SetIdCliente(idCliente);
            SetIdUsuario(idUsuario);
            SetNombreCompleto(nombreCompleto);
            SetDni(dni);
            SetTelefono(telefono);
            SetFechaNacimiento(fechaNacimiento);
            SetEstado(estado);
        }

        public void SetIdCliente(int idCliente)
        {
            if (idCliente < 0)
                throw new ArgumentException("El Id no puede ser negativo.", nameof(idCliente));
            IdCliente = idCliente;
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
                throw new ArgumentException("El nombre completo no puede estar vacío.", nameof(nombreCompleto));
            NombreCompleto = nombreCompleto;
        }

        public void SetDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new ArgumentException("El DNI no puede estar vacío.", nameof(dni));
            Dni = dni;
        }

        public void SetTelefono(string telefono)
        {
            Telefono = telefono; // Opcionalmente se podría validar que sean solo números
        }

        public void SetFechaNacimiento(DateTime fechaNacimiento)
        {
            // Validamos que el cliente haya nacido en el pasado
            if (fechaNacimiento >= DateTime.Today)
                throw new ArgumentException("La fecha de nacimiento debe ser en el pasado.", nameof(fechaNacimiento));
            FechaNacimiento = fechaNacimiento;
        }

        public void SetEstado(Estado estado)
        {
            Estado = estado;
        }
    }
}