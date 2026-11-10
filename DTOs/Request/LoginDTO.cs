using System.ComponentModel.DataAnnotations;

namespace MoerEsteticaBackend.DTOs.Request
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasenia { get; set; } = string.Empty;
    }
}