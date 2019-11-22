using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Repository
{
    public class PublisherRepositoryEF : RepositoryEF<Publisher>
    {
        public override int Add(Publisher item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                myDb.Publishers.Add(item);
                myDb.SaveChanges();
                return 1;
            }
        }
        public override bool Delete(Publisher item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemToDelete = myDb.Publishers.Find(item.Id);
                myDb.Publishers.Remove(itemToDelete);
                myDb.SaveChanges();
                return true;
            }
        }
        public override IEnumerable<Publisher> GetAll()
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var allItems = myDb.Publishers.ToList();
                return allItems;
            }
        }
        public override Publisher GetById(int id)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemById = myDb.Publishers.Find(id);
                return itemById;
            }
        }
        public override bool Update(Publisher item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemToUpdateById = myDb.Publishers.Find(item.Id);
                myDb.Entry(itemToUpdateById).CurrentValues.SetValues(item);
                // myDb.Entry(itemToUpdateById).State= EntityState.Modified;
                myDb.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// check if unique name of publisher exists
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsNameExist(Publisher item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemByName = myDb.Publishers.Where(x => x.PublisherName == item.PublisherName).FirstOrDefault();
                return itemByName == null ? false : true;
            }
        }
        public Publisher GetByPublisherLicense(int licenseNumber)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemByLicense = myDb.Publishers.Where(x =>x.LicenseNumber==licenseNumber).FirstOrDefault();
                return itemByLicense;
            }
        }
    }
}
