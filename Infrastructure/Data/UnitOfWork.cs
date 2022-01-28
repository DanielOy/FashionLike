using Core.Entities;
using Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FashionLikeContext _context;
        private bool disposed = false;

        public IGenericRepository<Tag> Tags { get; }
        public IGenericRepository<Post> Posts { get; }
        public IGenericRepository<Reaction> Reactions { get; }
        public IGenericRepository<Role> Roles { get; }
        public IGenericRepository<User> Users { get; }

        public UnitOfWork(FashionLikeContext context)
        {
            _context = context;

            Tags = new GenericRepository<Tag>(_context);
            Posts = new GenericRepository<Post>(_context);
            Reactions = new GenericRepository<Reaction>(_context);
            Roles = new GenericRepository<Role>(_context);
            Users = new GenericRepository<User>(_context);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
