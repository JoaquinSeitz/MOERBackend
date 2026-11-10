using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoerEsteticaBackend.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoTotal { get; set; }

        [Required]
        public int IdMetodoPago { get; set; }

        public int? IdPedido { get; set; }

        public int? IdTurno { get; set; }

        [ForeignKey("IdMetodoPago")]
        [JsonIgnore]
        public Metodo_Pago? Metodo_Pago { get; set; }

        [ForeignKey("IdPedido")]
        [JsonIgnore]
        public Pedido? Pedido { get; set; }

        [ForeignKey("IdTurno")]
        [JsonIgnore]
        public Turno? Turno { get; set; }
    }
}