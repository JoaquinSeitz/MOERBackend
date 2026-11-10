namespace MoerEsteticaBackend.DTOs.Response
{
    public class ServicioResponseDTO
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int IdCategoriaServicio { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}