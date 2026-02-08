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
    public class Startup
    {
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
