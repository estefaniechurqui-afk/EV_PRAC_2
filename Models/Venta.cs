namespace EV_PRAC_2.Models;

public class Venta
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public Producto? Producto { get; set; }
}
