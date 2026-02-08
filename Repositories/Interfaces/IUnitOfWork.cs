namespace NeorisBackend.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para la Unidad de Trabajo (Unit of Work)
    /// Coordina el trabajo de múltiples repositorios y gestiona las transacciones
    /// </summary>
    public interface IUnitOfWork
    {
        ILibroRepository Libros { get; }
        IAutorRepository Autores { get; }

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Libera los recursos
        /// </summary>
        void Dispose();
    }
}
