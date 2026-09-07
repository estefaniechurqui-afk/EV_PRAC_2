using System.ComponentModel.DataAnnotations;

namespace EV_PRAC_2.DTOs;

public class LoginRequest
{
    [Required]
    public string Usuario { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
