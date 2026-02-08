using System;

namespace NeorisBackend.DTOs.Responses
{
    /// <summary>
    /// DTO de respuesta para el login
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Token de acceso generado tras el login
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Fecha y hora UTC de expiración del token
        /// </summary>
        public DateTime ExpiresAtUtc { get; set; }
        /// <summary>
        /// Tipo de token (por ejemplo, "Bearer")
        /// </summary>
        public string TokenType { get; set; }
        
    }
}
