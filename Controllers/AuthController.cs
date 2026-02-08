using System;
using System.Web.Http;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.Services.Interfaces;
using Microsoft.Web.Http;

namespace NeorisBackend.Controllers
{
    /// <summary>
    /// Controlador de autenticacion - Refactorizado con Clean Architecture
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

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
    }
}
