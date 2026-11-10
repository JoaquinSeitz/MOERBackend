using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoerEsteticaBackend.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Contrasenia { get; set; } = string.Empty;

        [Required]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        [JsonIgnore]
        public Rol? Rol { get; set; }
    }
}