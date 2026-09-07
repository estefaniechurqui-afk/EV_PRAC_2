using EV_PRAC_2.Data;
using EV_PRAC_2.DTOs;
using EV_PRAC_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EV_PRAC_2.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetAll()
    {
        var categorias = await db.Categorias
            .AsNoTracking()
            .OrderBy(categoria => categoria.Nombre)
            .Select(categoria => new CategoriaResponse(categoria.Id, categoria.Nombre))
            .ToListAsync();

        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaResponse>> GetById(int id)
    {
        var categoria = await db.Categorias
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new CategoriaResponse(item.Id, item.Nombre))
            .SingleOrDefaultAsync();

        return categoria is null ? NotFound() : Ok(categoria);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CategoriaResponse>> Create(CategoriaRequest request)
    {
        var nombre = request.Nombre.Trim();
        if (await db.Categorias.AnyAsync(categoria => categoria.Nombre == nombre))
        {
            return Conflict("La categoría ya existe.");
        }

        var categoria = new Categoria { Nombre = nombre };
        db.Categorias.Add(categoria);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, new CategoriaResponse(categoria.Id, categoria.Nombre));
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoriaRequest request)
    {
        var categoria = await db.Categorias.FindAsync(id);
        if (categoria is null)
        {
            return NotFound();
        }

        var nombre = request.Nombre.Trim();
        if (await db.Categorias.AnyAsync(item => item.Id != id && item.Nombre == nombre))
        {
            return Conflict("La categoría ya existe.");
        }

        categoria.Nombre = nombre;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var categoria = await db.Categorias.FindAsync(id);
        if (categoria is null)
        {
            return NotFound();
        }

        if (await db.Productos.AnyAsync(producto => producto.CategoriaId == id))
        {
            return Conflict("No se puede eliminar una categoría que tiene productos.");
        }

        db.Categorias.Remove(categoria);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
