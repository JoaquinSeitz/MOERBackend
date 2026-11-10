using System.ComponentModel.DataAnnotations;

namespace MoerEsteticaBackend.Models
{
    public class Categoria_Producto
    {
        [Key]
        public int IdCategoriaProducto { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }
    }
}