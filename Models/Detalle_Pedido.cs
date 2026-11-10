using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoerEsteticaBackend.Models
{
    public class Detalle_Pedido
    {
        [Key]
        public int IdDetallePedido { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public int IdPedido { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [ForeignKey("IdPedido")]
        [JsonIgnore]
        public Pedido? Pedido { get; set; }

        [ForeignKey("IdProducto")]
        [JsonIgnore]
        public Producto? Producto { get; set; }
    }
}