using System;
using System.Linq;

namespace NeorisBackend.Extensions
{
    /// <summary>
    /// Extensiones útiles para el proyecto
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Verifica si una cadena es nula o vacía
        /// </summary>
        /// <param name="value">Cadena a evaluar</param>
        /// <returns>Verdadero si es nulo o vacío, falso en caso contrario</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Verifica si una cadena es nula, vacía o contiene solo espacios en blanco
        /// </summary>
        /// <param name="value">Cadena a evaluar</param>
        /// <returns>Verdadero si es nulo, vacío o solo espacios en blanco, falso en caso contrario</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
