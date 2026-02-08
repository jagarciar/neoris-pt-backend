using System.Data.Entity;
using NeorisBackend.Models;

namespace NeorisBackend.Data
{
    /// <summary>
    /// Contexto de Entity Framework para la base de datos NeorisPTDB
    /// </summary>
    public class NeorisDbContext : DbContext
    {
        /// <summary>
        /// Constructor que usa la cadena de conexión "DefaultConnection" del Web.config
        /// </summary>
        public NeorisDbContext() : base("name=DefaultConnection")
        {
            // Configuración para usar el esquema de base de datos existente
            Database.SetInitializer<NeorisDbContext>(null);
        }

        /// <summary>
        /// DbSet para la entidad Autores
        /// </summary>
        public virtual DbSet<Autor> Autores { get; set; }

        /// <summary>
        /// DbSet para la entidad Libros
        /// </summary>
        public virtual DbSet<Libro> Libros { get; set; }

        /// <summary>
        /// Configuración del modelo usando Fluent API
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuracion para Autores
            modelBuilder.Entity<Autor>()
                .ToTable("Autores")
                .HasKey(a => a.Id);

            modelBuilder.Entity<Autor>()
                .Property(a => a.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Autor>()
                .Property(a => a.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Autor>()
                .Property(a => a.CiudadProcedencia)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Autor>()
                .Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Autor>()
                .HasIndex(a => a.Email)
                .IsUnique();

            // Configuracion para Libros
            modelBuilder.Entity<Libro>()
                .ToTable("Libros")
                .HasKey(l => l.Id);

            modelBuilder.Entity<Libro>()
                .Property(l => l.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Libro>()
                .Property(l => l.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Libro>()
                .Property(l => l.Genero)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Libro>()
                .Property(l => l.NumeroPaginas)
                .IsRequired();

            modelBuilder.Entity<Libro>()
                .HasRequired(l => l.Autor)
                .WithMany()
                .HasForeignKey(l => l.AutorId);

            modelBuilder.Entity<Libro>()
                .HasIndex(l => l.AutorId);

            modelBuilder.Entity<Libro>()
                .HasIndex(l => l.Titulo);
        }
    }
}
