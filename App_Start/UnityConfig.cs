using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Unity;
using Unity.Lifetime;
using Unity.Exceptions;
using NeorisBackend.Data;
using NeorisBackend.Repositories.Interfaces;
using NeorisBackend.Repositories.Implementations;
using NeorisBackend.Services.Interfaces;
using NeorisBackend.Services.Implementations;

namespace NeorisBackend.App_Start
{
    /// <summary>
    /// Configuración del contenedor de inyección de dependencias Unity
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Registra los componentes y servicios en el contenedor de Unity
        /// </summary>
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Registrar DbContext (una instancia por request)
            container.RegisterType<NeorisDbContext>(new HierarchicalLifetimeManager());

            // Registrar Unit of Work
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());

            // Registrar Repositorios
            container.RegisterType<ILibroRepository, LibroRepository>();
            container.RegisterType<IAutorRepository, AutorRepository>();

            // Registrar Servicios
            container.RegisterType<ILibroService, LibroService>();
            container.RegisterType<IAutorService, AutorService>();
            container.RegisterType<IAuthService, AuthService>();

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }

   
}
