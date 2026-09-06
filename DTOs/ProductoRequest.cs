using System.ComponentModel.DataAnnotations;

namespace EV_PRAC_2.DTOs;

public class ProductoRequest
{
    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0.01, 999999.99)]
    public decimal Precio { get; set; }

    [Required, StringLength(80)]
    public string Categoria { get; set; } = string.Empty;

    [Url]
    public string? Imagen { get; set; }

    public bool Disponible { get; set; } = true;
}
