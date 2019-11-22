using AutoMapper;
using Dapper_BLL.Entities;
using Dapper_DAL;
using Dapper_DAL.Entities;
using EF_Repository;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_BLL
{
    public class PublisherService : BLLServices<BLLPublisher, Publisher>
    {
        public PublisherService()
        {
            this._curEFRep = new PublisherRepositoryEF();
        }
        public override int Add(BLLPublisher item)
        {
            // check if item with exists
            if (IsPublisherNameExists(item))
            {
                Console.WriteLine($"Genre with same name already exists");
                return 0;
            }
            int res = base.Add(item);
            return res;
        }
        public override bool Delete(BLLPublisher item)
        {
            if ((IsIdExists(item) == false) || (IsPubliserIncluded(item)))
            {
                return false;
            }
            return base.Delete(item);
        }
        public override bool Update(BLLPublisher item)
        {
            // check if item unique name already exists
            if (IsPublisherNameExists(item))
            {
                Console.WriteLine($"Publisher with same name already exists");
                return false;
            }
            bool res = base.Update(item);
            return res;
        }
        /// <summary>
        /// check if publihser with specific name already exists
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsPublisherNameExists(BLLPublisher item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BLLPublisher, Publisher>()).CreateMapper();
            var itemToAdd = mapper.Map<BLLPublisher, Publisher>(item);
            bool isGenreNameExist = (((PublisherRepositoryEF)_curEFRep).IsNameExist(itemToAdd));
            return isGenreNameExist;
        }
        /// <summary>
        /// check, if publisher has references in the games
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsPubliserIncluded(BLLPublisher item)
        {
            GameRepositoryEF gameRep = new GameRepositoryEF();
            bool isPublisherIncluded = gameRep.IsPublisherIncluded(item.Id);
            return isPublisherIncluded;
        }
    }
}
