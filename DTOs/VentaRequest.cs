using System.ComponentModel.DataAnnotations;

namespace EV_PRAC_2.DTOs;

public class VentaRequest
{
    [Range(1, int.MaxValue)]
    public int ProductoId { get; set; }

    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    public DateTime? Fecha { get; set; }
}
