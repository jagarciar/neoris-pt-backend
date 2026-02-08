using System.Configuration;

namespace NeorisBackend.Configuration
{
    /// <summary>
    /// Configuración centralizada para JWT
    /// </summary>
    public static class JwtConfig
    {
        /// <summary>
        /// Obtiene el público (audience) del token JWT desde el archivo de configuración (web.config). Asegúrate de que el archivo web.config contenga una sección <appSettings> con una entrada llamada "JwtAudience" que especifique el público de los tokens JWT. Esta propiedad proporciona un punto centralizado para acceder a esta configuración, facilitando su mantenimiento y actualización en un solo lugar si es necesario.
        /// </summary>
        public static string Audience => ConfigurationManager.AppSettings["JwtAudience"];
        /// <summary>
        /// Obtiene el tiempo de expiración en segundos para los tokens JWT desde el archivo de configuración (web.config). Asegúrate de que el archivo web.config contenga una sección <appSettings> con una entrada llamada "JwtExpirationSeconds" que especifique el tiempo de expiración en segundos para los tokens JWT. Esta propiedad proporciona un punto centralizado para acceder a esta configuración, facilitando su mantenimiento y actualización en un solo lugar si es necesario. El valor predeterminado es 3600 segundos (1 hora) si no se especifica en la configuración.
        /// </summary>
        public static int ExpirationSeconds => int.Parse(ConfigurationManager.AppSettings["JwtExpirationSeconds"] ?? "3600");
        /// <summary>
        /// Obtiene el emisor (issuer) del token JWT desde el archivo de configuración (web.config). Asegúrate de que el archivo web.config contenga una sección <appSettings> con una entrada llamada "JwtIssuer" que especifique el emisor de los tokens JWT. Esta propiedad proporciona un punto centralizado para acceder a esta configuración, facilitando su mantenimiento y actualización en un solo lugar si es necesario.
        /// </summary>
        public static string Issuer => ConfigurationManager.AppSettings["JwtIssuer"];
        /// <summary>
        /// Obtiene el secreto (secret) utilizado para firmar los tokens JWT desde el archivo de configuración (web.config). Asegúrate de que el archivo web.config contenga una sección <appSettings> con una entrada llamada "JwtSecret" que especifique un valor seguro y complejo para el secreto de los tokens JWT. Esta propiedad proporciona un punto centralizado para acceder a esta configuración, facilitando su mantenimiento y actualización en un solo lugar si es necesario. Es crucial que este valor sea lo suficientemente largo y complejo para garantizar la seguridad de los tokens JWT generados por la aplicación.
        /// </summary>
        public static string Secret => ConfigurationManager.AppSettings["JwtSecret"];
        
    }
}
