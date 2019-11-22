using AutoMapper;
using Dapper_BLL.Interfaces;
using Dapper_DAL;
using EF_Repository;
using EF_Repository.Interfaces;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_BLL
{
    //abstract generic class. Paramater T for communication with presentation level (BLL object), K - with DAL (DAL object)
    public abstract class BLLServices<T, K> : IGeneralService<T> where T : class, IEntityBLL where K : class, IEntity
    {
        protected RepositoryEF<K> _curEFRep;
        //just map enteties
        public virtual int Add(T item)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, K>()).CreateMapper();
                var itemToAdd = mapper.Map<T, K>(item);
                int addResult = _curEFRep.Add(itemToAdd);
                return addResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        public virtual bool Delete(T item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<T,K>()).CreateMapper();
            var itemToDelete = mapper.Map<T,K>(item);
            bool deleteResult = _curEFRep.Delete(itemToDelete);
            return deleteResult;
        }

        public virtual IEnumerable<T> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<K, T>()).CreateMapper();
            return mapper.Map<IEnumerable<K>, List<T>>(_curEFRep.GetAll());
        }
        public virtual T GetById(int id)
        {
            var publisherById = _curEFRep.GetById(id);
            if (publisherById != null)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<K, T>()).CreateMapper();
                return mapper.Map<K, T>(publisherById);
            }
            Console.WriteLine($"Item with Id={id} does not exist");
            return null;
        }
        //just map enteties
        public virtual bool Update(T item)
        {
            //return false, if we try to update item, which diesnt exist in DB
            if (IsIdExists(item) == false)
            {
                return false;
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, K>()).CreateMapper();
            var itemToUpdate = mapper.Map<T, K>(item);
            bool updResult = _curEFRep.Update(itemToUpdate);
            return updResult;
        }
        protected bool IsIdExists(T item)
        {
            var itemToFind = _curEFRep.GetById(item.Id);
            bool isIdExist = itemToFind == null ? false : true;
            return isIdExist;
        }
    }
}
