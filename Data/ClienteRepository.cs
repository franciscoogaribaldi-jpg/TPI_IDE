using Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ClienteRepository : IClienteRepository // implementamos a la clase ClienteRespository la interface
    {
        private readonly TPIContext _context; // creamos una caja vacia que solo acepta un obeto del tipo TPIContext y que al llenarse solo se va a poder leer(readonly)

        public ClienteRepository(TPIContext context)
        {
            _context = context; // llenamos la caja vacia con las dependencias provenientes de 
        }

        public async Task AddAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync(); // Aca es donde traduce a codigo SQL y lo guarda en la bd
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.IdCliente == id); // busca el cliente en la bd, con su usuario
            if (cliente == null || cliente.Estado == Estado.Inactivo) return false;

            // Baja lógica: el cliente queda en la base como Inactivo (sus reservas conservan el
            // historial) y su usuario también, así no puede seguir iniciando sesión.
            cliente.SetEstado(Estado.Inactivo);
            cliente.Usuario?.SetEstado(Estado.Inactivo);
            await _context.SaveChangesAsync(); // guarda el estado de la bd con el cliente borrado
            return true;
        }

        public async Task<Cliente?> GetAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.Usuario) // para que traiga tambien al Usuario asociado (seria un join)
                .FirstOrDefaultAsync(c => c.IdCliente == id); // el que tenga el mismo id
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            // Solo los activos: los eliminados (baja lógica) siguen en la base pero no se muestran.
            return await _context.Clientes
                .Include(c => c.Usuario)
                .Where(c => c.Estado == Estado.Activo)
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
            // El Estado no se cambia al editar: solo lo cambia la baja (DeleteAsync).

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DniExistsAsync(string dni, int? excludeId = null)
        {
            var query = _context.Clientes.Where(c => c.Dni.ToLower() == dni.ToLower()); // traduce tu sentencia a una query sql, es como una receta
           
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.IdCliente != excludeId.Value); // lo que hace aqui es aniadir un filtro extra para cuando excludeId tiene valor
            }
            return await query.AnyAsync(); // devuelve un boleano que dice si existe algun cliente que cumple con la query(true) sino false
        }

        public async Task<Cliente?> GetByIdUsuarioAsync(int idUsuario)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.IdUsuario == idUsuario);
        }

        public async Task<bool> ExisteConUsuarioAsync(int idUsuario)
        {
            return await _context.Clientes.AnyAsync(c => c.IdUsuario == idUsuario);
        }

        public async Task<Cliente?> GetByDniAsync(string dni)
        {
            return await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Dni.ToLower() == dni.Trim().ToLower());
        }

        public async Task RecuperarAsync(Cliente cliente)
        {
            // El cliente viene de GetByDniAsync, así que EF ya lo está siguiendo (a él y a su
            // usuario): alcanza con guardar los cambios, los dos en un único SaveChangesAsync.
            await _context.SaveChangesAsync();
        }

        /* 
           <summary>
            Requisito técnico de la cátedra: usar ADO.NET puro al menos una vez
            (el resto del acceso a datos usa EF Core). Se aplica acá, en la
            búsqueda con filtros, igual que en el ejemplo resuelto de la cátedra,
            pero con Microsoft.Data.SqlClient (el conector vigente que recomienda
            el material de Unidad 4 - SQL Server; System.Data.SqlClient está en
            modo mantenimiento).
           </summary> 
        */
        public async Task<IEnumerable<Cliente>> GetByCriteriaAsync(ClienteCriteria criteria)
        {
            // JOIN con Usuarios para traer también quién registró al cliente
            // (el equivalente al .Include(c => c.Usuario) que usan GetAllAsync y GetAsync).
            const string sql = @"
        SELECT c.IdCliente, c.IdUsuario, c.Nombre, c.Apellido, c.Dni, c.Telefono,
               c.FechaNacimiento, c.Estado,
               u.NombreUsuario, u.Contrasena, u.Email, u.Rol, u.Estado AS EstadoUsuario
        FROM Clientes c
        INNER JOIN Usuarios u ON u.IdUsuario = c.IdUsuario
        WHERE c.Estado = 1
          AND (c.Nombre LIKE @Busqueda
               OR c.Apellido LIKE @Busqueda
               OR c.Dni LIKE @Busqueda)
        ORDER BY c.Apellido, c.Nombre";

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

                var usuario = new Usuario(
                    idUsuario: reader.GetInt32(1),
                    nombreUsuario: reader.GetString(8),
                    contrasena: reader.GetString(9),
                    email: reader.GetString(10),
                    rol: (RolUsuario)reader.GetInt32(11),
                    estado: (Estado)reader.GetInt32(12));

                cliente.SetUsuario(usuario); // ahora c.Usuario ya no es null

                clientes.Add(cliente);
            }

            return clientes;
        }
    }
}
