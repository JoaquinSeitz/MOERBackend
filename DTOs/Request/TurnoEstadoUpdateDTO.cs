using System.ComponentModel.DataAnnotations;

namespace MoerEstetica.API.DTOs.Request
{
    public class TurnoEstadoUpdateDTO
    {
        [Required(ErrorMessage = "El estado del turno es obligatorio.")]
        public int IdEstadoTurno { get; set; }
    }
}