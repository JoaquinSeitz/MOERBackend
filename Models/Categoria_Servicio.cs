using System.ComponentModel.DataAnnotations;

namespace MoerEsteticaBackend.Models
{
    public class Categoria_Servicio
    {
        [Key]
        public int IdCategoriaServicio { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Descripcion { get; set; }
    }
}