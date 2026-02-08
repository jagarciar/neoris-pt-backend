using System;
using System.Collections.Generic;
using System.Linq;

namespace NeorisBackend.Extensions
{
    /// <summary>
    /// Extensiones para IEnumerable
    /// </summary>
    public static class EnumerableExtensions
    {

        /// <summary>
        /// Ejecuta una acción para cada elemento de la colección
        /// </summary>
        /// <typeparam name="T">Tipo de dato</typeparam>
        /// <param name="source">Fuente de datos</param>
        /// <param name="action">Acción a ejecutar</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || action == null)
                return;

            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Verifica si una colección es nula o está vacía
        /// </summary>
        /// <typeparam name="T">Tipo de dato</typeparam>
        /// <param name="source">Fuente de datos</param>
        /// <returns>Verdadero si es nulo o vacío</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        
    }
}
