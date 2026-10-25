using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly TPIContext _context;

        public AdministradorRepository(TPIContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Administrador administrador)
        {
            // El administrador viene con su Usuario nuevo adentro (SetUsuario): EF inserta los
            // dos en un único SaveChangesAsync, así no queda uno guardado sin el otro.
            _context.Administradores.Add(administrador);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var administrador = await _context.Administradores
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.IdAdministrador == id);
            if (administrador == null || administrador.Estado == Estado.Inactivo) return false;

            // Baja lógica: la ficha queda en la base como Inactiva (las reservas que registró
            // conservan el dato) y su usuario también, así no puede seguir iniciando sesión.
            administrador.SetEstado(Estado.Inactivo);
            administrador.Usuario?.SetEstado(Estado.Inactivo);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Administrador?> GetAsync(int id)
        {
            return await _context.Administradores
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.IdAdministrador == id);
        }

        public async Task<Administrador?> GetByIdUsuarioAsync(int idUsuario)
        {
            return await _context.Administradores
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.IdUsuario == idUsuario);
        }

        public async Task<IEnumerable<Administrador>> GetAllAsync()
        {
            // Solo los activos: los eliminados (baja lógica) siguen en la base pero no se muestran.
            return await _context.Administradores
                .Include(a => a.Usuario)
                .Where(a => a.Estado == Estado.Activo)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Administrador administrador)
        {
            var existente = await _context.Administradores.FindAsync(administrador.IdAdministrador);
            if (existente == null) return false;

            existente.SetNombre(administrador.Nombre);
            existente.SetApellido(administrador.Apellido);
            existente.SetTelefono(administrador.Telefono);
            // El Estado no se cambia al editar: solo lo cambia la baja (DeleteAsync).

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RecuperarAsync(Administrador administrador)
        {
            // Viene de GetByIdUsuarioAsync, así que EF ya lo está siguiendo (a él y a su usuario):
            // alcanza con guardar los cambios, los dos en un único SaveChangesAsync.
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteConUsuarioAsync(int idUsuario)
        {
            return await _context.Administradores.AnyAsync(a => a.IdUsuario == idUsuario);
        }
    }
}
