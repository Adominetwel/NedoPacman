using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DALInterfaces
{
    public interface IDomainRepository<T> where T : IDomainObject, new()
    {
        void Create(T obj);
        IEnumerable<T> ReadAll();
        T ReadById(int id);
        void Update(T obj);
        void Delete(T obj);
    }
}
    