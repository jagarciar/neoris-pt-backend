using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Unity;

namespace NeorisBackend.App_Start
{
    /// <summary>
    /// Implementación personalizada de IDependencyResolver para Unity Container
    /// </summary>
    public class UnityDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Refencia al contenedor de Unity utilizado para resolver dependencias. Se inyecta a través del constructor y se utiliza para resolver servicios y crear scopes de dependencia.
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Constructor que recibe una instancia del contenedor de Unity. Se asegura de que el contenedor no sea nulo, lanzando una excepción si lo es. Esta instancia se utiliza para resolver dependencias a lo largo del ciclo de vida de la aplicación.
        /// </summary>
        /// <param name="container">Contenedor de Unity a utilizar para la resolución de dependencias.</param>
        /// <exception cref="ArgumentNullException">Excepción que se lanza cuando el contenedor de Unity es nulo.</exception>
        public UnityDependencyResolver(IUnityContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Inicia un nuevo scope de dependencia creando un contenedor hijo a partir del contenedor principal. Esto permite que las dependencias resueltas dentro de este scope tengan una vida útil limitada al scope, lo que es especialmente útil para manejar dependencias con tiempos de vida específicos, como las dependencias por solicitud en aplicaciones web.
        /// </summary>
        /// <returns>Conntenedor hijo creado a partir del contenedor principal para gestionar un scope de dependencia.</returns>
        public IDependencyScope BeginScope()
        {
            var child = _container.CreateChildContainer();
            return new UnityDependencyResolver(child);
        }

        /// <summary>
        /// Libera los recursos utilizados por el contenedor de Unity. Si el contenedor es nulo, no se realiza ninguna acción. Esto es importante para asegurar que los recursos gestionados por el contenedor, como las instancias de servicios, se liberen adecuadamente cuando ya no sean necesarios, evitando posibles fugas de memoria y asegurando un manejo eficiente de los recursos en la aplicación.
        /// </summary>
        public void Dispose()
        {
            _container?.Dispose();
        }

        /// <summary>
        /// Permite resolver una instancia de un servicio dado su tipo. Si el servicio no está registrado en el contenedor, se captura la excepción de resolución fallida y se devuelve null, lo que permite que el sistema de inyección de dependencias maneje la ausencia del servicio de manera adecuada.
        /// </summary>
        /// <param name="serviceType">Tipo del servicio a resolver.</param>
        /// <returns>Si el servicio se puede resolver, se devuelve la instancia; de lo contrario, se devuelve null.</returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Retorna todas las instancias de un servicio dado su tipo. Si el servicio no está registrado en el contenedor, se captura la excepción de resolución fallida y se devuelve una colección vacía, lo que permite que el sistema de inyección de dependencias maneje la ausencia de servicios de manera adecuada.
        /// </summary>
        /// <param name="serviceType">Tipo del servicio a resolver.</param>
        /// <returns>Lista de todas las instancias del servicio solicitado o una colección vacía si no se encuentran.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return Enumerable.Empty<object>();
            }
        }

        

        
    }
}