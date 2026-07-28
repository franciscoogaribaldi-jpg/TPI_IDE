using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Usuario
    {
        public int IdUsuario { get; private set; }
        public string NombreUsuario { get; private set; }
        public string Contrasena { get; private set; }
        public string Email { get; private set; }
        public RolUsuario Rol { get; private set; }
        public Estado Estado { get; private set; }

        
        public Usuario(int idUsuario, string nombreUsuario, string contrasena, string email, RolUsuario rol, Estado estado)
        {
            SetIdUsuario(idUsuario);
            SetNombreUsuario(nombreUsuario);
            SetContrasena(contrasena);
            SetEmail(email);
            SetRol(rol);
            SetEstado(estado);
        }

        public void SetIdUsuario(int idUsuario)
        {
            if (idUsuario < 0)
                throw new ArgumentException("El Id no puede ser negativo.", nameof(idUsuario));
            IdUsuario = idUsuario;
        }

        public void SetNombreUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.", nameof(nombreUsuario));
            NombreUsuario = nombreUsuario;
        }

        public void SetContrasena(string contrasena)
        {
            if (string.IsNullOrWhiteSpace(contrasena))
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(contrasena));
            Contrasena = contrasena;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("El email no tiene un formato válido.", nameof(email));
            Email = email;
        }

        public void SetRol(RolUsuario rol)
        {
            Rol = rol;
        }

        public void SetEstado(Estado estado)
        {
            Estado = estado;
        }
    }
}