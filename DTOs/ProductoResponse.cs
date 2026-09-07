namespace EV_PRAC_2.DTOs;

public record ProductoResponse(
    int Id,
    string Nombre,
    string Descripcion,
    decimal Precio,
    string Categoria,
    int? CategoriaId,
    string? Imagen,
    bool Disponible);
