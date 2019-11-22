using EF_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Repository
{
    public abstract class RepositoryEF<T> : ICommonRepositoryEF<T> where T : class, IEntity
    {
        public abstract int Add(T item);


        public abstract bool Delete(T item);


        public abstract IEnumerable<T> GetAll();


        public abstract T GetById(int id);


        public abstract bool Update(T item);

    }
}
