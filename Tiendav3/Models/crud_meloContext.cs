using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tiendav3.Models
{
    public partial class crud_meloContext : IdentityDbContext
    {
        public crud_meloContext()
        {
        }

        public crud_meloContext(DbContextOptions<crud_meloContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Factura> Facturas { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=crud_melo;integrated security=True; TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Cedula)
                    .HasName("PK__Clientes__B4ADFE3973FC4286");

                entity.Property(e => e.Cedula).ValueGeneratedNever();

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                base.OnModelCreating(modelBuilder);
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(e => e.NumeroFactura)
                    .HasName("PK__Facturas__CF12F9A7196169CC");

                entity.Property(e => e.NumeroFactura).ValueGeneratedNever();

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.HasOne(d => d.ClienteCedulaNavigation)
                    .WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.ClienteCedula)
                    .HasConstraintName("FK__Facturas__Client__3B75D760");

                entity.HasOne(d => d.ProductoCodigoNavigation)
                    .WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.ProductoCodigo)
                    .HasConstraintName("FK__Facturas__Produc__3C69FB99");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Codigo)
                    .HasName("PK__Producto__06370DADC44F1744");

                entity.Property(e => e.Codigo).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
