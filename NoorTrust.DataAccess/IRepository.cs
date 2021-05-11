using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoorTrust.DataAccess
{
    public interface IRepository<T> where T : IInt32Identity
    {


        IList<T> GetAll();
        Task<IList<T>> GetAllAsync();

        T GetById(int id);
        Task<int> CountAsync();
        Task<int> CountActiveAsync();
        //  Task<IList<T>> GetByIdAsync(int id);

        void Save(T saveThis);
        void Delete(T deleteThis);
        int CountActive();
    }
}
