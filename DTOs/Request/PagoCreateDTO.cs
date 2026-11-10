using System.ComponentModel.DataAnnotations;

namespace MoerEstetica.API.DTOs.Request
{
    public class PagoCreateDTO
    {
        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public int IdMetodoPago { get; set; }

        public int? IdPedido { get; set; }

        public int? IdTurno { get; set; }
    }
}