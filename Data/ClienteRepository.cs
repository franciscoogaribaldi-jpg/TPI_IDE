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
            var cliente = await _context.Clientes.FindAsync(id); // busca el cliente en la bd
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
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
            var query = _context.Clientes.Where(c => c.Dni.ToLower() == dni.ToLower()); // traduce tu sentencia a una query sql, es como una receta
           
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.IdCliente != excludeId.Value); // lo que hace aqui es aniadir un filtro extra para cuando excludeId tiene valor
            }
            return await query.AnyAsync(); // devuelve un boleano que dice si existe algun cliente que cumple con la query(true) sino false
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
            const string sql = @"
                SELECT IdCliente, IdUsuario, Nombre, Apellido, Dni, Telefono, FechaNacimiento, Estado
                FROM Clientes
                WHERE Nombre LIKE @Busqueda
                   OR Apellido LIKE @Busqueda
                   OR Dni LIKE @Busqueda
                ORDER BY Apellido, Nombre";

            


            var clientes = new List<Cliente>();
            string? connectionString = _context.Database.GetConnectionString(); // aprovechamos el EF para pedir el ConnectionString para usar ADO.Net. El connectionString es la ubicacion de la Db
            string patron = $"%{criteria.Texto}%";

            using var connection = new SqlConnection(connectionString); // Crea el cable físico de internet para enchufarse a la base de datos usando la ruta de conexión
            using var command = new SqlCommand(sql, connection); // Es el cartero. Le entregamos nuestro texto SQL crudo y le decimos por qué cable tiene que viajar.

            command.Parameters.AddWithValue("@Busqueda", patron); // Le inyectamos la palabra que el usuario buscó (@Busqueda). @ porque es variable temporal

            await connection.OpenAsync(); // Esto es para abrir la coneccion con la bd
            using var reader = await command.ExecuteReaderAsync(); // ejecutar el command y devuelve un reader que no son todos los datos de una

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
