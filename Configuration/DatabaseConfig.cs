using System.Configuration;

namespace NeorisBackend.Configuration
{
    /// <summary>
    /// Configuración centralizada para la base de datos
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// Obtiene la cadena de conexión a la base de datos desde el archivo de configuración (web.config). Asegúrate de que el archivo web.config contenga una sección <connectionStrings> con una entrada llamada "DefaultConnection" que apunte a tu base de datos SQL Server. Esta propiedad proporciona un punto centralizado para acceder a la cadena de conexión, facilitando su mantenimiento y actualización en un solo lugar si es necesario.
        /// </summary>
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
    }
}
