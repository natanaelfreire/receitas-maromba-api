using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceitasMarombaApi.Interfaces
{
    public interface ICrudRepository<T>
    {
        IEnumerable<T> GetAll();
        T? GetOne(int id);
        bool Create(T data);
        bool Update(int id, T data);
        bool Delete(int id);
    }
}