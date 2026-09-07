namespace EV_PRAC_2.Models;

public class Usuario
{
    public int Id { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}
