using System;

namespace MoerEstetica.API.DTOs.Response
{
    public class PagoResponseDTO
    {
        public int IdPago { get; set; }
        public DateTime Fecha { get; set; }
        public decimal MontoTotal { get; set; }
        public string MetodoPago { get; set; }
        public int? IdPedido { get; set; }
        public int? IdTurno { get; set; }
    }
}