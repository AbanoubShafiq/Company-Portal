using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
        IGenericRepository<T> GenericRepository<T>() where T : class;
    }
}
