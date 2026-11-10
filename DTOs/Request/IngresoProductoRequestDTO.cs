using System.ComponentModel.DataAnnotations;

namespace MoerEsteticaBackend.DTOs.Request
{
    public class IngresoProductoRequestDTO
    {
        public List<DetalleIngresoRequestDTO> Detalles { get; set; } = new List<DetalleIngresoRequestDTO>();
    }

    public class DetalleIngresoRequestDTO
    {
        [Required(ErrorMessage = "El ID del producto es obligatorio.")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio de compra unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor a 0.")]
        public decimal PrecioCompraUnitario { get; set; }
    }
}