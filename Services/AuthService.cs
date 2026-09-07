using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EV_PRAC_2.Data;
using EV_PRAC_2.DTOs;
using EV_PRAC_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EV_PRAC_2.Services;

public class AuthService(
    AppDbContext db,
    IPasswordHasher<Usuario> passwordHasher,
    IConfiguration configuration)
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var usuario = await db.Usuarios
            .SingleOrDefaultAsync(item => item.UsuarioNombre == request.Usuario && item.Activo);

        if (usuario is null || passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.PasswordHash,
                request.Password) == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var expiration = DateTime.UtcNow.AddHours(8);
        var claims = new[]
        {
            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new System.Security.Claims.Claim(ClaimTypes.Name, usuario.UsuarioNombre),
            new System.Security.Claims.Claim("nombre", usuario.NombreCompleto)
        };

        var key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("No está configurada la clave JWT.");
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new LoginResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiration,
            usuario.UsuarioNombre,
            usuario.NombreCompleto);
    }
}
