using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoerEsteticaBackend.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public int StockActual { get; set; } = 0;

        [MaxLength(500)]
        public string? Imagen { get; set; }

        public bool Activo { get; set; } = true;

        [Required]
        public int IdCategoriaProducto { get; set; }

        [ForeignKey("IdCategoriaProducto")]
        [JsonIgnore]
        public Categoria_Producto? Categoria_Producto { get; set; }
    }
}