namespace MoerEsteticaBackend.DTOs.Response
{
    public class IngresoProductoResponseDTO
    {
        public int IdIngreso { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetalleIngresoResponseDTO> Detalles { get; set; } = new List<DetalleIngresoResponseDTO>();
    }

    public class DetalleIngresoResponseDTO
    {
        public int IdProducto { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioCompraUnitario { get; set; }
    }
}