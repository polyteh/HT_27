using EF_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Repository
{
    public class GenreRepositoryEF : RepositoryEF<Genre>
    {
        public GenreRepositoryEF()
        {
        }
        public override int Add(Genre item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                myDb.Genres.Add(item);
                myDb.SaveChanges();
                return 1;
            }
        }
        public override bool Delete(Genre item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemToUpdateById = myDb.Genres.Find(item.Id);
                myDb.Genres.Remove(item);
                myDb.SaveChanges();
                return true;
            }
        }
        public override IEnumerable<Genre> GetAll()
        {
            using (ComputerGamesEntities myDb=new ComputerGamesEntities())
            {
                var allItems = myDb.Genres.ToList();
                return allItems;
            }
        }
        public override Genre GetById(int id)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemById = myDb.Genres.Find(id);
                return itemById;
            }
        }
        public override bool Update(Genre item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemToUpdateById = myDb.Genres.Find(item.Id);
                myDb.Entry(itemToUpdateById).CurrentValues.SetValues(item);
                myDb.SaveChanges();
                return true;
            }
        }
        public bool IsNameExist(Genre item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemByName = myDb.Genres.Where(x => x.GenreName==item.GenreName).FirstOrDefault();
                return itemByName == null?false:true;
            }
        }
    }
}
