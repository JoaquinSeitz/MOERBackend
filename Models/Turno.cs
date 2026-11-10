using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoerEsteticaBackend.Models
{
    public class Turno
    {
        [Key]
        public int IdTurno { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraInicio { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int IdServicio { get; set; }

        [Required]
        public int IdEstadoTurno { get; set; }

        [ForeignKey("IdCliente")]
        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [ForeignKey("IdServicio")]
        [JsonIgnore]
        public Servicio? Servicio { get; set; }

        [ForeignKey("IdEstadoTurno")]
        [JsonIgnore]
        public Estado_Turno? Estado_Turno { get; set; }
    }
}