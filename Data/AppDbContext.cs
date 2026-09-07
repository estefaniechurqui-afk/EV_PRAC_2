using EV_PRAC_2.Models;
using Microsoft.EntityFrameworkCore;

namespace EV_PRAC_2.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>()
            .Property(producto => producto.Precio)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Venta>()
            .Property(venta => venta.Total)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Venta>()
            .HasOne(venta => venta.Producto)
            .WithMany(producto => producto.Ventas)
            .HasForeignKey(venta => venta.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Producto>()
            .HasOne(producto => producto.CategoriaRelacionada)
            .WithMany(categoria => categoria.Productos)
            .HasForeignKey(producto => producto.CategoriaId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Categoria>()
            .HasIndex(categoria => categoria.Nombre)
            .IsUnique();
    }
}
