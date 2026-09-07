using EV_PRAC_2.Data;
using EV_PRAC_2.Models;
using Microsoft.EntityFrameworkCore;

namespace EV_PRAC_2.Services;

public static class CategoriaSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        var nombres = await db.Productos
            .Where(producto => producto.CategoriaId == null && producto.Categoria != string.Empty)
            .Select(producto => producto.Categoria)
            .Distinct()
            .ToListAsync();

        foreach (var nombre in nombres)
        {
            var categoria = await db.Categorias.SingleOrDefaultAsync(item => item.Nombre == nombre);
            if (categoria is null)
            {
                categoria = new Categoria { Nombre = nombre };
                db.Categorias.Add(categoria);
                await db.SaveChangesAsync();
            }

            await db.Productos
                .Where(producto => producto.CategoriaId == null && producto.Categoria == nombre)
                .ExecuteUpdateAsync(setters => setters.SetProperty(producto => producto.CategoriaId, categoria.Id));
        }
    }
}
