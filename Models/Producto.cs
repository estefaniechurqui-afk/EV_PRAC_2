namespace EV_PRAC_2.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string? Imagen { get; set; }
    public bool Disponible { get; set; } = true;

    public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
