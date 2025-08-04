using DAL.Context;
using Repository.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private Hashtable repositories;

        public UnitOfWork(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            if (repositories is null)
                repositories = new Hashtable();

            var entityKey = typeof(T).Name;

            if (!repositories.ContainsKey(entityKey))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);

                repositories.Add(entityKey, repositoryInstance);
            }
            return (IGenericRepository<T>)repositories[entityKey];
        }
    }

}
