namespace EV_PRAC_2.DTOs;

public record VentaResponse(
    int Id,
    int ProductoId,
    string Producto,
    int Cantidad,
    decimal Total,
    DateTime Fecha);
