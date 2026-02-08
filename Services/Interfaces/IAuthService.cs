using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Responses;

namespace NeorisBackend.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de Autenticación
    /// Contiene la lógica de negocio relacionada con autenticación
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica un usuario y genera un JWT
        /// </summary>
        LoginResponseDto Login(LoginRequestDto dto);

        /// <summary>
        /// Valida las credenciales del usuario
        /// </summary>
        bool ValidateCredentials(string username, string password);
    }
}
