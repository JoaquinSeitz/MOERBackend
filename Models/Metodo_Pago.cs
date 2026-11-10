using System.ComponentModel.DataAnnotations;

namespace MoerEsteticaBackend.Models
{
    public class Metodo_Pago
    {
        [Key]
        public int IdMetodoPago { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}