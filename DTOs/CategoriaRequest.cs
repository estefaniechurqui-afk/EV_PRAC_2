using System.ComponentModel.DataAnnotations;

namespace EV_PRAC_2.DTOs;

public class CategoriaRequest
{
    [Required, StringLength(80)]
    public string Nombre { get; set; } = string.Empty;
}
