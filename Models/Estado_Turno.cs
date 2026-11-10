using System.ComponentModel.DataAnnotations;

namespace MoerEsteticaBackend.Models
{
    public class Estado_Turno
    {
        [Key]
        public int IdEstadoTurno { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }
    }
}