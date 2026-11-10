using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoerEsteticaBackend.Models
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public int IdEstadoPedido { get; set; }

        [ForeignKey("IdCliente")]
        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [ForeignKey("IdEstadoPedido")]
        [JsonIgnore]
        public Estado_Pedido? Estado_Pedido { get; set; }

        public List<Detalle_Pedido> Detalles { get; set; } = new List<Detalle_Pedido>();
    }
}