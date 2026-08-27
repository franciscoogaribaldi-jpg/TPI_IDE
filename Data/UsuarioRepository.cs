using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TPIContext _context;

        public UsuarioRepository(TPIContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario?> GetAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            var existente = await _context.Usuarios.FindAsync(usuario.IdUsuario);
            if (existente == null) return false;

            existente.SetNombreUsuario(usuario.NombreUsuario);
            existente.SetEmail(usuario.Email);
            existente.SetRol(usuario.Rol);
            existente.SetEstado(usuario.Estado);
            // La contraseña NO se toca acá: se cambia con un flujo aparte (fuera del alcance de la Entrega 2).

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NombreUsuarioExistsAsync(string nombreUsuario, int? excludeId = null)
        {
            var query = _context.Usuarios.Where(u => u.NombreUsuario.ToLower() == nombreUsuario.ToLower());
            if (excludeId.HasValue)
                query = query.Where(u => u.IdUsuario != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _context.Usuarios.Where(u => u.Email.ToLower() == email.ToLower());
            if (excludeId.HasValue)
                query = query.Where(u => u.IdUsuario != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
