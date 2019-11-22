using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Repository.Interfaces
{
    public interface ICommonRepositoryEF<T>
    {
        T GetById(int id);
        bool Update(T item);
        bool Delete(T item);
        int Add(T item);
        IEnumerable<T> GetAll();
    }
}
