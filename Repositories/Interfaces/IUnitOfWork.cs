namespace NeorisBackend.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz para la Unidad de Trabajo (Unit of Work)
    /// Coordina el trabajo de múltiples repositorios y gestiona las transacciones
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Referencia al repositorio de autores, que proporciona acceso a las operaciones CRUD para los autores. Esta propiedad se inicializa de manera perezosa (lazy loading) para optimizar el rendimiento y evitar la creación innecesaria de instancias de repositorios hasta que realmente se necesiten. Al utilizar esta propiedad, todas las operaciones realizadas a través del repositorio de autores compartirán el mismo contexto de base de datos, lo que garantiza la coherencia de los datos en todas las operaciones realizadas a través de esta unidad de trabajo.
        /// </summary>
        IAutorRepository Autores { get; }
        /// <summary>
        /// Referencia al repositorio de libros, que proporciona acceso a las operaciones CRUD para los libros. Esta propiedad se inicializa de manera perezosa (lazy loading) para optimizar el rendimiento y evitar la creación innecesaria de instancias de repositorios hasta que realmente se necesiten. Al utilizar esta propiedad, todas las operaciones realizadas a través del repositorio de libros compartirán el mismo contexto de base de datos, lo que garantiza la coherencia de los datos en todas las operaciones realizadas a través de esta unidad de trabajo.
        /// </summary>
        ILibroRepository Libros { get; }
        

        /// <summary>
        /// Libera los recursos
        /// </summary>
        void Dispose();

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos
        /// </summary>
        int SaveChanges();

        
    }
}
