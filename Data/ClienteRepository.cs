using Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly TPIContext _context;

        public ClienteRepository(TPIContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Cliente?> GetAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.IdCliente == id);
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Include(c => c.Usuario)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            var existing = await _context.Clientes.FindAsync(cliente.IdCliente);
            if (existing == null) return false;

            existing.SetNombre(cliente.Nombre);
            existing.SetApellido(cliente.Apellido);
            existing.SetDni(cliente.Dni);
            existing.SetTelefono(cliente.Telefono);
            existing.SetFechaNacimiento(cliente.FechaNacimiento);
            existing.SetEstado(cliente.Estado);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DniExistsAsync(string dni, int? excludeId = null)
        {
            var query = _context.Clientes.Where(c => c.Dni.ToLower() == dni.ToLower());
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.IdCliente != excludeId.Value);
            }
            return await query.AnyAsync();
        }

        /// <summary>
        /// Requisito técnico de la cátedra: usar ADO.NET puro al menos una vez
        /// (el resto del acceso a datos usa EF Core). Se aplica acá, en la
        /// búsqueda con filtros, igual que en el ejemplo resuelto de la cátedra,
        /// pero con Microsoft.Data.SqlClient (el conector vigente que recomienda
        /// el material de Unidad 4 - SQL Server; System.Data.SqlClient está en
        /// modo mantenimiento).
        /// </summary>
        public async Task<IEnumerable<Cliente>> GetByCriteriaAsync(ClienteCriteria criteria)
        {
            const string sql = @"
                SELECT IdCliente, IdUsuario, Nombre, Apellido, Dni, Telefono, FechaNacimiento, Estado
                FROM Clientes
                WHERE Nombre LIKE @Busqueda
                   OR Apellido LIKE @Busqueda
                   OR Dni LIKE @Busqueda
                ORDER BY Apellido, Nombre";

            var clientes = new List<Cliente>();
            string? connectionString = _context.Database.GetConnectionString();
            string patron = $"%{criteria.Texto}%";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Busqueda", patron);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var cliente = new Cliente(
                    idCliente: reader.GetInt32(0),
                    idUsuario: reader.GetInt32(1),
                    nombre: reader.GetString(2),
                    apellido: reader.GetString(3),
                    dni: reader.GetString(4),
                    telefono: reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                    fechaNacimiento: reader.GetDateTime(6),
                    estado: (Estado)reader.GetInt32(7));

                clientes.Add(cliente);
            }

            return clientes;
        }
    }
}
