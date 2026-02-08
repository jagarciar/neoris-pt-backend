using System.ComponentModel.DataAnnotations;

namespace NeorisBackend.DTOs.Requests
{
    /// <summary>
    /// DTO para la solicitud de login
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Contraseña del usuario
        /// </summary>

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string Username { get; set; }
    }
}
