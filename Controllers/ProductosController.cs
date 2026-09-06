using EV_PRAC_2.Data;
using EV_PRAC_2.DTOs;
using EV_PRAC_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EV_PRAC_2.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetAll()
    {
        var productos = await db.Productos
            .AsNoTracking()
            .Select(producto => ToResponse(producto))
            .ToListAsync();

        return Ok(productos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoResponse>> GetById(int id)
    {
        var producto = await db.Productos
            .AsNoTracking()
            .Where(producto => producto.Id == id)
            .Select(producto => ToResponse(producto))
            .SingleOrDefaultAsync();

        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProductoResponse>> Create(ProductoRequest request)
    {
        var producto = new Producto();
        UpdateEntity(producto, request);

        db.Productos.Add(producto);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, ToResponse(producto));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, ProductoRequest request)
    {
        var producto = await db.Productos.FindAsync(id);
        if (producto is null)
        {
            return NotFound();
        }

        UpdateEntity(producto, request);
        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await db.Productos.FindAsync(id);
        if (producto is null)
        {
            return NotFound();
        }

        if (await db.Ventas.AnyAsync(venta => venta.ProductoId == id))
        {
            return Conflict("No se puede eliminar un producto que tiene ventas registradas.");
        }

        db.Productos.Remove(producto);
        await db.SaveChangesAsync();

        return NoContent();
    }

    private static void UpdateEntity(Producto producto, ProductoRequest request)
    {
        producto.Nombre = request.Nombre.Trim();
        producto.Descripcion = request.Descripcion.Trim();
        producto.Precio = request.Precio;
        producto.Categoria = request.Categoria.Trim();
        producto.Imagen = request.Imagen;
        producto.Disponible = request.Disponible;
    }

    private static ProductoResponse ToResponse(Producto producto) => new(
        producto.Id,
        producto.Nombre,
        producto.Descripcion,
        producto.Precio,
        producto.Categoria,
        producto.Imagen,
        producto.Disponible);
}
