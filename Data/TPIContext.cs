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
        public DbSet<Cancha> Canchas { get; set; } = null!;

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
                      .WithMany()
                      .HasForeignKey(c => c.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);
            });

           
            modelBuilder.Entity<Cancha>(entity =>
            {
                entity.ToTable("Canchas");
                entity.HasKey(c => c.IdCancha);
                entity.Property(c => c.IdCancha).ValueGeneratedOnAdd();

                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Estado).IsRequired().HasConversion<int>();
                entity.Property(c => c.PrecioPorHora).HasColumnType("decimal(18,2)");

                entity.HasDiscriminator<string>("TipoCancha")
                      .HasValue<CanchaFutbol>("Futbol")
                      .HasValue<CanchaPadel>("Padel");
            });

            modelBuilder.Entity<CanchaPadel>(entity =>
            {
                entity.Property(c => c.PrecioTotalRaquetas).HasColumnType("decimal(18,2)");
            });
        }
    }
}
