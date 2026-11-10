using System.ComponentModel.DataAnnotations;

namespace MoerEsteticaBackend.Models
{
    public class Estado_Pedido
    {
        [Key]
        public int IdEstadoPedido { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }
    }
}