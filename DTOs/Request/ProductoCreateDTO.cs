using System.ComponentModel.DataAnnotations;

namespace MoerEsteticaBackend.DTOs.Request
{
    public class ProductoCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock inicial es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int StockActual { get; set; }

        [MaxLength(255)]
        public string? Imagen { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        public int IdCategoriaProducto { get; set; }
    }
}