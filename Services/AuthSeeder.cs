using EV_PRAC_2.Data;
using EV_PRAC_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EV_PRAC_2.Services;

public static class AuthSeeder
{
    public static async Task SeedAsync(
        AppDbContext db,
        IPasswordHasher<Usuario> passwordHasher)
    {
        if (await db.Usuarios.AnyAsync())
        {
            return;
        }

        var usuario = new Usuario
        {
            UsuarioNombre = "admin",
            NombreCompleto = "Administrador",
            Activo = true
        };
        usuario.PasswordHash = passwordHasher.HashPassword(usuario, "Admin123!");

        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync();
    }
}
