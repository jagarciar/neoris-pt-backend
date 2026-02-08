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
        /// Permite a un usuario iniciar sesión y obtener un token de acceso
        /// </summary>
        /// <param name="dto">DTO de inicio de sesión</param>
        /// <returns>DTO de respuesta de inicio de sesión</returns>
        LoginResponseDto Login(LoginRequestDto dto);

        /// <summary>
        /// Permite validar las credenciales de un usuario, verificando si el nombre de usuario y la contraseña proporcionados son correctos. Este método es fundamental para el proceso de autenticación, ya que asegura que solo los usuarios con credenciales válidas puedan acceder a los recursos protegidos de la aplicación. La implementación de este método puede involucrar la consulta a una base de datos para verificar las credenciales contra los registros de usuarios almacenados, y puede incluir medidas adicionales de seguridad, como el hashing de contraseñas para proteger la información sensible.
        /// </summary>
        /// <param name="username">Usuario a validar</param>
        /// <param name="password">Contraseña a validar</param>
        /// <returns>Verdadero si las credenciales son válidas, falso en caso contrario</returns>
        bool ValidateCredentials(string username, string password);
    }
}
