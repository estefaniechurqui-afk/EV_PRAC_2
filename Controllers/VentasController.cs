using EV_PRAC_2.Data;
using EV_PRAC_2.DTOs;
using EV_PRAC_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EV_PRAC_2.Controllers;

[ApiController]
[Authorize]
[Route("api/ventas")]
public class VentasController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VentaResponse>>> GetAll()
    {
        var ventas = await db.Ventas
            .AsNoTracking()
            .Include(venta => venta.Producto)
            .OrderByDescending(venta => venta.Fecha)
            .Select(venta => ToResponse(venta))
            .ToListAsync();

        return Ok(ventas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VentaResponse>> GetById(int id)
    {
        var venta = await db.Ventas
            .AsNoTracking()
            .Include(venta => venta.Producto)
            .Where(venta => venta.Id == id)
            .Select(venta => ToResponse(venta))
            .SingleOrDefaultAsync();

        return venta is null ? NotFound() : Ok(venta);
    }

    [HttpPost]
    public async Task<ActionResult<VentaResponse>> Create(VentaRequest request)
    {
        var producto = await db.Productos.FindAsync(request.ProductoId);
        if (producto is null)
        {
            return BadRequest("El producto indicado no existe.");
        }

        if (!producto.Disponible)
        {
            return Conflict("El producto no está disponible para la venta.");
        }

        var venta = new Venta
        {
            ProductoId = producto.Id,
            Cantidad = request.Cantidad,
            Total = producto.Precio * request.Cantidad,
            Fecha = request.Fecha ?? DateTime.UtcNow
        };

        db.Ventas.Add(venta);
        await db.SaveChangesAsync();

        venta.Producto = producto;
        return CreatedAtAction(nameof(GetById), new { id = venta.Id }, ToResponse(venta));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, VentaRequest request)
    {
        var venta = await db.Ventas.FindAsync(id);
        if (venta is null)
        {
            return NotFound();
        }

        var producto = await db.Productos.FindAsync(request.ProductoId);
        if (producto is null)
        {
            return BadRequest("El producto indicado no existe.");
        }

        if (!producto.Disponible)
        {
            return Conflict("El producto no está disponible para la venta.");
        }

        venta.ProductoId = producto.Id;
        venta.Cantidad = request.Cantidad;
        venta.Total = producto.Precio * request.Cantidad;
        venta.Fecha = request.Fecha ?? venta.Fecha;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var venta = await db.Ventas.FindAsync(id);
        if (venta is null)
        {
            return NotFound();
        }

        db.Ventas.Remove(venta);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static VentaResponse ToResponse(Venta venta) => new(
        venta.Id,
        venta.ProductoId,
        venta.Producto?.Nombre ?? string.Empty,
        venta.Cantidad,
        venta.Total,
        venta.Fecha);
}
