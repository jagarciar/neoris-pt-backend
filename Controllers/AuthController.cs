using System;
using System.Web.Http;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.Services.Interfaces;
using Microsoft.Web.Http;

namespace NeorisBackend.Controllers
{
    /// <summary>
    /// Controlador de autenticación
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/auth")]
    public class AuthController : ApiController
    {
        /// <summary>
        /// Refencia al servicio de autenticación, que maneja la lógica de negocio relacionada con el inicio de sesión y la generación de tokens JWT.
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor del controlador de autenticación, que recibe una instancia de IAuthService a través de inyección de dependencias para garantizar un diseño modular y testable.
        /// </summary>
        /// <param name="authService">Servicio de autenticación</param>
        /// <exception cref="ArgumentNullException">Genera excepción cuando el servicio de autenticación es nulo</exception>
        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        /// <summary>
        /// Permite a los usuarios autenticados obtener información sobre su propia cuenta, como su nombre de usuario. Esta acción requiere que el usuario esté autenticado, y devuelve un error de autorización si no lo está.
        /// </summary>
        /// <returns>Nombre de usuario autenticado</returns>
        [HttpGet]
        [Authorize]
        [Route("me")]
        public IHttpActionResult Me()
        {
            var name = User?.Identity?.Name;

            return Ok(new
            {
                username = name
            });
        }

        /// <summary>
        /// Permite a los usuarios iniciar sesión proporcionando su nombre de usuario y contraseña. Si las credenciales son válidas, devuelve un token JWT que el cliente puede usar para autenticarse en futuras solicitudes. Si las credenciales son inválidas, devuelve un error de autenticación.
        /// </summary>
        /// <param name="request">Solicitud de inicio de sesión con nombre de usuario y contraseña</param>
        /// <returns>Token de acceso JWT o error de autenticación</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Usuario y clave son requeridos");
            }

            try
            {
                var response = _authService.Login(request);

                if (response == null)
                {
                    return Unauthorized();
                }

                return Ok(new
                {
                    accessToken = response.AccessToken,
                    tokenType = response.TokenType,
                    expiresAtUtc = response.ExpiresAtUtc
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al autenticar: {ex.Message}"));
            }
        }

       
    }
}
