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
    public class TPIContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Administrador> Administradores { get; set; } = null!;
        public DbSet<TipoSuperficie> TiposSuperficie { get; set; } = null!;
        public DbSet<Cancha> Canchas { get; set; } = null!;
        public DbSet<Turno> Turnos { get; set; } = null!;
        public DbSet<Reserva> Reservas { get; set; } = null!;
        public DbSet<DetalleReserva> DetallesReserva { get; set; } = null!;

        public TPIContext(DbContextOptions<TPIContext> options) : base(options)
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

                entity.HasOne(c => c.Usuario)
                      .WithMany()
                      .HasForeignKey(c => c.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- Administrador ----------
            modelBuilder.Entity<Administrador>(entity =>
            {
                entity.ToTable("Administradores");
                entity.HasKey(a => a.IdAdministrador);
                entity.Property(a => a.IdAdministrador).ValueGeneratedOnAdd();

                entity.Property(a => a.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Telefono).HasMaxLength(30);
                entity.Property(a => a.Estado).IsRequired().HasConversion<int>();

                entity.HasIndex(a => a.IdUsuario).IsUnique();

                entity.HasOne(a => a.Usuario)
                      .WithMany()
                      .HasForeignKey(a => a.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- TipoSuperficie (Tipo de Cancha) ----------
            modelBuilder.Entity<TipoSuperficie>(entity =>
            {
                entity.ToTable("TiposSuperficie");
                entity.HasKey(t => t.IdTipoSuperficie);
                entity.Property(t => t.IdTipoSuperficie).ValueGeneratedOnAdd();

                entity.Property(t => t.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(t => t.Deporte).IsRequired().HasMaxLength(50);
                entity.Property(t => t.Estado).IsRequired().HasConversion<int>();
            });

            // ---------- Cancha ----------
            modelBuilder.Entity<Cancha>(entity =>
            {
                entity.ToTable("Canchas");
                entity.HasKey(c => c.IdCancha);
                entity.Property(c => c.IdCancha).ValueGeneratedOnAdd();

                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Estado).IsRequired().HasConversion<int>();
                entity.Property(c => c.PrecioPorHora).HasColumnType("decimal(18,2)");
                entity.Property(c => c.EsTechada).IsRequired();

                entity.Ignore(c => c.CantidadJugadores);
                entity.Ignore(c => c.Caracteristicas);

                entity.HasOne(c => c.TipoSuperficie)
                      .WithMany()
                      .HasForeignKey(c => c.IdTipoSuperficie)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasDiscriminator<string>("TipoCancha")
                      .HasValue<CanchaFutbol>("Futbol")
                      .HasValue<CanchaPadel>("Padel");
            });

            // ---------- Cancha Futbol ----------
            modelBuilder.Entity<CanchaFutbol>(entity =>
            {
                entity.Property(c => c.TipoFutbol).HasConversion<int>();
            });

            // ---------- Cancha Padel ----------
            modelBuilder.Entity<CanchaPadel>();

            // ---------- Turno ----------
            modelBuilder.Entity<Turno>(entity =>
            {
                entity.ToTable("Turnos");
                entity.HasKey(t => t.IdTurno);
                entity.Property(t => t.IdTurno).ValueGeneratedOnAdd();

                entity.Property(t => t.Estado).IsRequired().HasConversion<int>();
                entity.Property(t => t.HoraInicio).IsRequired();
                entity.Property(t => t.HoraFin).IsRequired();
            });

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
                entity.HasOne(r => r.Administrador).WithMany().HasForeignKey(r => r.IdAdministrador).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
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
                      .WithMany(r => r.Detalles)
                      .HasForeignKey(d => d.IdReserva)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
