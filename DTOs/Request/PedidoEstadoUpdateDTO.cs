using System.ComponentModel.DataAnnotations;

namespace MoerEstetica.API.DTOs.Request
{
    public class PedidoEstadoUpdateDTO
    {
        [Required(ErrorMessage = "El estado del pedido es obligatorio.")]
        public int IdEstadoPedido { get; set; }
    }
}