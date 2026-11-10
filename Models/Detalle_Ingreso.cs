using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoerEsteticaBackend.Models
{
    public class Detalle_Ingreso
    {
        [Key]
        public int IdDetalleIngreso { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioCompraUnitario { get; set; }

        [Required]
        public int IdIngreso { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [ForeignKey("IdIngreso")]
        [JsonIgnore]
        public Ingreso_Stock? Ingreso_Stock { get; set; }

        [ForeignKey("IdProducto")]
        [JsonIgnore]
        public Producto? Producto { get; set; }
    }
}