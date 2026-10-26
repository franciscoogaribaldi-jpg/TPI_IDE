using Application.Services;
using Data;
using Domain.Model;

namespace WebAPI
{
    /// <summary>
    /// Datos de prueba representativos (requisito de la Entrega Final): administradores,
    /// clientes con su usuario, tipos de cancha/superficie, canchas de los dos tipos y reservas
    /// pasadas y futuras con líneas, para que todas las pantallas y los reportes muestren algo.
    /// Cada bloque se carga solo si su tabla está vacía, así no se duplica nada al reiniciar la API.
    /// Contraseñas: administradores "admin123", clientes "cliente123".
    /// </summary>
    public static class DatosDePrueba
    {
        public static void Cargar(TPIContext context)
        {
            CargarAdministradores(context);
            CargarClientes(context);
            CargarTiposSuperficie(context);
            CargarCanchas(context);
            CargarReservas(context);
        }

        private static void CargarAdministradores(TPIContext context)
        {
            if (context.Administradores.Any()) return;

            // Ficha del usuario Admin inicial (lo crea Program.cs).
            var usuarioAdmin = context.Usuarios.FirstOrDefault(u => u.NombreUsuario == "Admin");
            if (usuarioAdmin != null)
            {
                var fichaAdmin = new Administrador(0, 0, "Administrador", "General", "341-4000000", Estado.Activo);
                fichaAdmin.SetUsuario(usuarioAdmin);
                context.Administradores.Add(fichaAdmin);
            }

            var segundoAdmin = new Administrador(0, 0, "Laura", "Gómez", "341-4111111", Estado.Activo);
            segundoAdmin.SetUsuario(NuevoUsuario("lgomez", "lgomez@complejo.com", "admin123", RolUsuario.Administrador));
            context.Administradores.Add(segundoAdmin);

            context.SaveChanges();
        }

        private static void CargarClientes(TPIContext context)
        {
            if (context.Clientes.Any()) return;

            var clientes = new[]
            {
                ("Juan", "Pérez", "30111222", "341-5550001", new DateTime(1990, 3, 15), "jperez"),
                ("María", "García", "32444555", "341-5550002", new DateTime(1993, 7, 22), "mgarcia"),
                ("Lucas", "Fernández", "35777888", "341-5550003", new DateTime(1998, 11, 5), "lfernandez"),
                ("Sofía", "Martínez", "37123456", "341-5550004", new DateTime(2000, 1, 30), "smartinez"),
                ("Diego", "Rodríguez", "28999000", "341-5550005", new DateTime(1985, 9, 9), "drodriguez"),
                ("Valentina", "López", "39222333", "341-5550006", new DateTime(2002, 5, 18), "vlopez")
            };

            foreach (var (nombre, apellido, dni, telefono, nacimiento, usuario) in clientes)
            {
                var cliente = new Cliente(0, 0, nombre, apellido, dni, telefono, nacimiento, Estado.Activo);
                cliente.SetUsuario(NuevoUsuario(usuario, $"{usuario}@mail.com", "cliente123", RolUsuario.Cliente));
                context.Clientes.Add(cliente);
            }

            context.SaveChanges();
        }

        private static void CargarTiposSuperficie(TPIContext context)
        {
            if (context.TiposSuperficie.Any()) return;

            context.TiposSuperficie.AddRange(
                new TipoSuperficie(0, "Césped sintético", "Futbol", Estado.Activo),
                new TipoSuperficie(0, "Césped natural", "Futbol", Estado.Activo),
                new TipoSuperficie(0, "Cemento", "Futbol", Estado.Activo),
                new TipoSuperficie(0, "Parquet", "Futbol", Estado.Activo),
                new TipoSuperficie(0, "Vidrio / Blindex", "Padel", Estado.Activo),
                new TipoSuperficie(0, "Muro / Cemento", "Padel", Estado.Activo));

            context.SaveChanges();
        }

        private static void CargarCanchas(TPIContext context)
        {
            if (context.Canchas.Any()) return;

            var tipos = context.TiposSuperficie.ToList();
            int idSintetico = tipos.FirstOrDefault(t => t.Nombre == "Césped sintético")?.IdTipoSuperficie ?? 1;
            int idNatural = tipos.FirstOrDefault(t => t.Nombre == "Césped natural")?.IdTipoSuperficie ?? 2;
            int idCemento = tipos.FirstOrDefault(t => t.Nombre == "Cemento")?.IdTipoSuperficie ?? 3;
            int idVidrio = tipos.FirstOrDefault(t => t.Nombre.StartsWith("Vidrio"))?.IdTipoSuperficie ?? 5;
            int idMuro = tipos.FirstOrDefault(t => t.Nombre.StartsWith("Muro"))?.IdTipoSuperficie ?? 6;

            context.Canchas.AddRange(
                new CanchaFutbol(0, "Cancha 1", Estado.Activo, 30000, true, idSintetico, TipoFutbol.Futbol5),
                new CanchaFutbol(0, "Cancha 2", Estado.Activo, 28000, false, idSintetico, TipoFutbol.Futbol5),
                new CanchaFutbol(0, "Cancha 3", Estado.Activo, 45000, false, idNatural, TipoFutbol.Futbol7),
                new CanchaFutbol(0, "Cancha 4", Estado.Inactivo, 25000, true, idCemento, TipoFutbol.Futbol5),
                new CanchaPadel(0, "Pádel 1", Estado.Activo, 20000, true, idVidrio),
                new CanchaPadel(0, "Pádel 2", Estado.Activo, 16000, false, idMuro));

            context.SaveChanges();
        }

        private static void CargarReservas(TPIContext context)
        {
            if (context.Reservas.Any()) return;

            var clientes = context.Clientes.ToList();
            var canchas = context.Canchas.Where(c => c.Estado == Estado.Activo).ToList();
            var turnos = context.Turnos.Where(t => t.Estado == Estado.Activo).ToList();
            var administradores = context.Administradores.ToList();
            if (!clientes.Any() || !canchas.Any() || !turnos.Any()) return;

            // Semilla fija: siempre se generan las mismas reservas (datos repetibles para probar).
            var azar = new Random(2024);
            var ocupados = new HashSet<(int, int, DateTime)>(); // cancha + turno + fecha: no se repite

            // Desde hace 45 días hasta dentro de 14 días.
            for (int dia = -45; dia <= 14; dia++)
            {
                var fecha = DateTime.Today.AddDays(dia);
                int reservasDelDia = azar.Next(1, 5);

                for (int i = 0; i < reservasDelDia; i++)
                {
                    var cancha = canchas[azar.Next(canchas.Count)];
                    // Más reservas a la tarde-noche, como en un complejo real.
                    var turno = turnos[Math.Min(turnos.Count - 1, azar.Next(turnos.Count / 3, turnos.Count))];
                    if (!ocupados.Add((cancha.IdCancha, turno.IdTurno, fecha))) continue;

                    var cliente = clientes[azar.Next(clientes.Count)];
                    var lineas = ArmarLineas(cancha, azar);
                    decimal importeTotal = cancha.PrecioPorHora + lineas.Sum(l => l.Subtotal);

                    EstadoReserva estado = dia < 0
                        ? (azar.Next(10) == 0 ? EstadoReserva.Cancelada : EstadoReserva.Finalizada)
                        : (azar.Next(2) == 0 ? EstadoReserva.Pendiente : EstadoReserva.Confirmada);

                    decimal sena = estado == EstadoReserva.Pendiente ? 0 : Math.Round(importeTotal * 0.3m);

                    var reserva = new Reserva(0, cliente.IdCliente, cancha.IdCancha, turno.IdTurno, fecha, estado, importeTotal, sena);
                    foreach (var linea in lineas)
                        reserva.AgregarDetalle(linea);

                    if (administradores.Any())
                        reserva.SetIdAdministrador(administradores[azar.Next(administradores.Count)].IdAdministrador);

                    context.Reservas.Add(reserva);
                }
            }

            context.SaveChanges();
        }

        private static List<DetalleReserva> ArmarLineas(Cancha cancha, Random azar)
        {
            var lineas = new List<DetalleReserva>();

            if (cancha is CanchaPadel)
            {
                // En pádel casi siempre se alquilan raquetas o se compran pelotas.
                if (azar.Next(10) < 7)
                {
                    int raquetas = azar.Next(1, 5); // 1 a 4 raquetas
                    lineas.Add(new DetalleReserva(0, 0, "Alquiler raqueta de pádel", raquetas, 2500));
                }
                if (azar.Next(10) < 4)
                {
                    lineas.Add(new DetalleReserva(0, 0, "Tubo de pelotas de pádel", 1, 6000));
                }
            }
            else
            {
                // En fútbol pecheras o pelota.
                if (azar.Next(10) < 5)
                {
                    lineas.Add(new DetalleReserva(0, 0, "Alquiler juego de pecheras", 1, 3000));
                }
                if (azar.Next(10) < 4)
                {
                    lineas.Add(new DetalleReserva(0, 0, "Alquiler pelota de fútbol", 1, 2000));
                }
            }

            // Bebidas en cualquier tipo de cancha.
            if (azar.Next(10) < 6)
            {
                int aguas = azar.Next(1, 4);
                lineas.Add(new DetalleReserva(0, 0, "Agua mineral 500ml", aguas, 1200));
            }

            return lineas;
        }

        private static Usuario NuevoUsuario(string nombreUsuario, string email, string contrasenaPlana, RolUsuario rol)
        {
            return new Usuario(0, nombreUsuario, PasswordHasher.Hash(contrasenaPlana), email, rol, Estado.Activo);
        }
    }
}
