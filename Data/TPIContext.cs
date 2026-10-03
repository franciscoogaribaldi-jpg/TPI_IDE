using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    /// <summary>
    /// DbContext de EF Core para el sistema. Se configura desde WebAPI/Program.cs
    /// (AddDbContext), leyendo la cadena de conexión de appsettings.json.
    /// La base se autogenera si no existe (Database.EnsureCreated, llamado una
    /// única vez al iniciar la app en Program.cs).
    /// </summary>
    public class TPIContext : DbContext // TPIContext hereda de DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Cancha> Canchas { get; set; } = null!;
        public DbSet<Turno> Turnos { get; set; } = null!;
        public DbSet<Reserva> Reservas { get; set; } = null!;
        public DbSet<DetalleReserva> DetallesReserva { get; set; } = null!;

        public TPIContext(DbContextOptions<TPIContext> options) : base(options) // Constructor de TPIContext que recibe las opciones y se las envía a la clase padre DbContext
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Usuario ----------
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(u => u.IdUsuario);
                entity.Property(u => u.IdUsuario).ValueGeneratedOnAdd();

                entity.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(50);
                // Guardamos el HASH de la contrasenia, hecho en applicationServices
                entity.Property(u => u.Contrasena).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
                entity.Property(u => u.Rol).IsRequired().HasConversion<int>();
                entity.Property(u => u.Estado).IsRequired().HasConversion<int>();

                entity.HasIndex(u => u.NombreUsuario).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();


            });

            // ---------- Cliente ----------
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Clientes");
                entity.HasKey(c => c.IdCliente);
                entity.Property(c => c.IdCliente).ValueGeneratedOnAdd();

                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Dni).IsRequired().HasMaxLength(20);
                entity.Property(c => c.Telefono).HasMaxLength(30);
                entity.Property(c => c.Estado).IsRequired().HasConversion<int>();

                entity.HasIndex(c => c.Dni).IsUnique();

                // Cliente.Usuario e IdUsuario tienen setter privado: EF puede usarlo igual
                // por reflexión, no hace falta HasField() como en el ejemplo de cátedra.
                entity.HasOne(c => c.Usuario)
                      .WithMany() // no lo tenemos en cuenta porque No me importa registrar la relación a la inversa adentro de la clase Usuario, solo me importa desde el lado del Cliente
                      .HasForeignKey(c => c.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);   // para que cuando quieras borrar el usuario primero tengas que borrar el cliente asignado
            });

            // ---------- Cancha ----------
            modelBuilder.Entity<Cancha>(entity =>
            {
                entity.ToTable("Canchas");
                entity.HasKey(c => c.IdCancha);
                entity.Property(c => c.IdCancha).ValueGeneratedOnAdd();

                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Estado).IsRequired().HasConversion<int>();
                entity.Property(c => c.PrecioPorHora).HasColumnType("decimal(18,2)"); // nro de 18 digitos con 2 decimales que representan los centavos

                entity.HasDiscriminator<string>("TipoCancha")
                      .HasValue<CanchaFutbol>("Futbol")
                      .HasValue<CanchaPadel>("Padel");
            });

            // ---------- Cancha Padel ----------
            modelBuilder.Entity<CanchaPadel>(entity =>
            {
                entity.Property(c => c.PrecioTotalRaquetas).HasColumnType("decimal(18,2)");
            });


            // ---------- Turno ----------
            modelBuilder.Entity<Turno>(entity =>
            {
                entity.ToTable("Turnos");
                entity.HasKey(t => t.IdTurno);
                entity.Property(t => t.IdTurno).ValueGeneratedOnAdd();

                entity.Property(t => t.Estado).IsRequired().HasConversion<int>();
                entity.Property(t => t.HoraInicio).IsRequired();
                entity.Property(t => t.HoraFin).IsRequired();
            }
            
            );


            // ---------- Reserva ----------
            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("Reservas");
                entity.HasKey(r => r.IdReserva);
                entity.Property(r => r.IdReserva).ValueGeneratedOnAdd();

                entity.Property(r => r.EstadoReserva).IsRequired().HasConversion<int>();
                entity.Property(r => r.ImporteTotal).HasColumnType("decimal(18,2)");
                entity.Property(r => r.Sena).HasColumnType("decimal(18,2)");

                entity.HasOne(r => r.Cliente).WithMany().HasForeignKey(r => r.IdCliente).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(r => r.Cancha).WithMany().HasForeignKey(r => r.IdCancha).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(r => r.Turno).WithMany().HasForeignKey(r => r.IdTurno).OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- DetalleReserva ----------
            modelBuilder.Entity<DetalleReserva>(entity =>
            {
                entity.ToTable("DetallesReserva");
                entity.HasKey(d => d.IdDetalleReserva);
                entity.Property(d => d.IdDetalleReserva).ValueGeneratedOnAdd();

                entity.Property(d => d.Concepto).IsRequired().HasMaxLength(100);
                entity.Property(d => d.PrecioUnitario).HasColumnType("decimal(18,2)");
                entity.Ignore(d => d.Subtotal);

                entity.HasOne<Reserva>()
                      .WithMany()
                      .HasForeignKey(d => d.IdReserva)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
