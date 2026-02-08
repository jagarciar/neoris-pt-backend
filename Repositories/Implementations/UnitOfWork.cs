using System;
using NeorisBackend.Data;
using NeorisBackend.Repositories.Interfaces;

namespace NeorisBackend.Repositories.Implementations
{
    /// <summary>
    /// Coordina la interacción entre los repositorios y el contexto de datos, asegurando que las operaciones de base de datos se realicen de manera consistente y eficiente.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NeorisDbContext _context;
        private ILibroRepository _libros;
        private IAutorRepository _autores;

        public UnitOfWork(NeorisDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ILibroRepository Libros
        {
            get
            {
                if (_libros == null)
                {
                    _libros = new LibroRepository(_context);
                }
                return _libros;
            }
        }

        public IAutorRepository Autores
        {
            get
            {
                if (_autores == null)
                {
                    _autores = new AutorRepository(_context);
                }
                return _autores;
            }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
