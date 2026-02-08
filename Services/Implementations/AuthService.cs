using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Responses;
using NeorisBackend.Services.Interfaces;

namespace NeorisBackend.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de Autenticación
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Audiencia del token JWT, que representa a quién está destinado el token. Este valor se utiliza para validar que el token recibido en las solicitudes posteriores sea válido y esté destinado a esta aplicación. Se carga desde el archivo de configuración para facilitar su mantenimiento y actualización.
        /// </summary>
        private readonly string _audience;
        /// <summary>
        /// Contraseña esperada para la autenticación, que se utiliza para validar las credenciales proporcionadas por el cliente durante el proceso de inicio de sesión. Este valor se carga desde el archivo de configuración para permitir una fácil modificación sin necesidad de recompilar el código. Es importante configurar este valor adecuadamente para garantizar la seguridad de la autenticación en la aplicación.
        /// </summary>
        private readonly string _expectedPassword;
        /// <summary>
        /// Usuario esperado para la autenticación, que se utiliza para validar las credenciales proporcionadas por el cliente durante el proceso de inicio de sesión. Este valor se carga desde el archivo de configuración para permitir una fácil modificación sin necesidad de recompilar el código. Es importante configurar este valor adecuadamente para garantizar la seguridad de la autenticación en la aplicación.
        /// </summary>
        private readonly string _expectedUsername;
        /// <summary>
        /// Segundos de expiración para los tokens JWT, que determina cuánto tiempo serán válidos los tokens generados por la aplicación. Este valor se utiliza para establecer la fecha de expiración del token al momento de su creación, y se carga desde el archivo de configuración para permitir una fácil modificación sin necesidad de recompilar el código. Es importante configurar este valor adecuadamente para equilibrar la seguridad y la usabilidad de la autenticación basada en JWT en la aplicación.
        /// </summary>
        private readonly int _expirationSeconds;
        /// <summary>
        /// Entrada de configuración para el servicio de autenticación, que incluye el emisor del token, la audiencia, la clave secreta para firmar el token, el tiempo de expiración del token, y las credenciales esperadas para validar el inicio de sesión. Estos valores se cargan desde el archivo de configuración de la aplicación (app.config o web.config) para permitir una fácil modificación sin necesidad de recompilar el código.
        /// </summary>
        private readonly string _issuer;
        /// <summary>
        /// Llave secreta utilizada para firmar los tokens JWT, que garantiza la integridad y autenticidad de los tokens generados por la aplicación. Este valor debe ser lo suficientemente largo y complejo para asegurar la seguridad de los tokens JWT. Se carga desde el archivo de configuración para facilitar su mantenimiento y actualización sin necesidad de modificar el código fuente. Es crucial proteger este valor y no exponerlo públicamente, ya que es fundamental para la seguridad de la autenticación basada en JWT en la aplicación.
        /// </summary>
        private readonly string _secret;

        /// <summary>
        /// Constructor del servicio de autenticación, que carga las configuraciones necesarias para la generación y validación de tokens JWT, así como las credenciales esperadas para el inicio de sesión. Estas configuraciones se obtienen del archivo de configuración de la aplicación (app.config o web.config) para permitir una fácil modificación sin necesidad de recompilar el código. Es importante asegurarse de que estas configuraciones estén correctamente establecidas en el archivo de configuración para garantizar el correcto funcionamiento del servicio de autenticación.
        /// </summary>
        public AuthService()
        {
            _issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            _audience = ConfigurationManager.AppSettings["JwtAudience"];
            _secret = ConfigurationManager.AppSettings["JwtSecret"];
            _expirationSeconds = int.Parse(ConfigurationManager.AppSettings["JwtExpirationSeconds"]);
            _expectedUsername = ConfigurationManager.AppSettings["AuthUsername"];
            _expectedPassword = ConfigurationManager.AppSettings["AuthPassword"];
        }

        /// <summary>
        /// Permite a los usuarios autenticarse proporcionando un nombre de usuario y contraseña. Si las credenciales son válidas, se genera un token JWT que contiene la información del usuario y se devuelve al cliente. Este token puede ser utilizado en solicitudes posteriores para acceder a recursos protegidos en la aplicación. El método también maneja la validación de las credenciales proporcionadas por el usuario, asegurándose de que solo los usuarios autorizados puedan obtener un token válido. Es importante que las credenciales esperadas estén configuradas correctamente en el archivo de configuración para garantizar la seguridad del proceso de autenticación.
        /// </summary>
        /// <param name="dto">DTO de inicio de sesión con las credenciales del usuario</param>
        /// <returns>DTO de respuesta de inicio de sesión con el token generado</returns>
        public LoginResponseDto Login(LoginRequestDto dto)
        {
            if (!ValidateCredentials(dto.Username, dto.Password))
            {
                return null;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var expires = now.AddSeconds(_expirationSeconds);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: new[] { new Claim(ClaimTypes.Name, dto.Username) },
                notBefore: now,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseDto
            {
                AccessToken = tokenString,
                TokenType = "Bearer",
                ExpiresAtUtc = expires
            };
        }

        /// <summary>
        /// Permite validar las credenciales proporcionadas por el usuario durante el proceso de inicio de sesión. Compara el nombre de usuario y la contraseña proporcionados con los valores esperados configurados en el archivo de configuración de la aplicación. Si las credenciales coinciden, devuelve true, indicando que el usuario está autenticado correctamente. Si las credenciales no coinciden, devuelve false, lo que indica que el intento de autenticación ha fallado. Es importante que los valores esperados para el nombre de usuario y la contraseña estén configurados correctamente en el archivo de configuración para garantizar la seguridad del proceso de autenticación.
        /// </summary>
        /// <param name="username">Usuario proporcionado por el cliente</param>
        /// <param name="password">Contraseña proporcionada por el cliente</param>
        /// <returns>Verdadero si las credenciales son válidas, falso en caso contrario</returns>
        public bool ValidateCredentials(string username, string password)
        {
            return string.Equals(username, _expectedUsername, StringComparison.Ordinal) &&
                   string.Equals(password, _expectedPassword, StringComparison.Ordinal);
        }
    }
}
