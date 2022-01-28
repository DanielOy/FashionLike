using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Post> Posts { get; }
        IGenericRepository<Reaction> Reactions { get; }
        IGenericRepository<Tag> Tags { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<User> Users { get; }

        Task Save();
    }
}
