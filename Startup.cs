using System.Configuration;
using System.Text;
using System.Web.Http;
using System.Web.Http.Owin;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;

[assembly: OwinStartup(typeof(NeorisBackend.Startup))]

namespace NeorisBackend
{
    /// <summary>
    /// Configura la autenticación JWT para la aplicación utilizando OWIN Middleware. Este método se ejecuta al iniciar la aplicación y establece los parámetros de validación del token, incluyendo el emisor, el público, la clave de firma y la duración del token. Además, se asegura de que las solicitudes a la API sean autenticadas correctamente utilizando tokens JWT, lo que proporciona una capa adicional de seguridad para proteger los endpoints de la API contra accesos no autorizados.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configura la autenticación JWT para la aplicación utilizando OWIN Middleware. Este método se ejecuta al iniciar la aplicación y establece los parámetros de validación del token, incluyendo el emisor, el público, la clave de firma y la duración del token. Además, se asegura de que las solicitudes a la API sean autenticadas correctamente utilizando tokens JWT, lo que proporciona una capa adicional de seguridad para proteger los endpoints de la API contra accesos no autorizados.
        /// </summary>
        /// <param name="app">Constructor de la aplicación OWIN.</param>
        public void Configuration(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;
            if (config.Routes.Count == 0)
            {
                WebApiConfig.Register(config);
            }

            var issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            var audience = ConfigurationManager.AppSettings["JwtAudience"];
            var secret = ConfigurationManager.AppSettings["JwtSecret"];
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = true,
                    ClockSkew = System.TimeSpan.FromMinutes(2)
                }
            });

            app.UseWebApi(config);
        }
    }
}
