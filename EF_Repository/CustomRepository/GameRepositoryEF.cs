using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EF_Repository
{
    public class GameRepositoryEF : RepositoryEF<Game>
    {
        public override int Add(Game item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                myDb.Games.Add(item);
                myDb.SaveChanges();
                return 1;
            }
        }

        public override bool Delete(Game item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                myDb.Games.Remove(item);
                myDb.SaveChanges();
                return true;
            }
        }

        public override IEnumerable<Game> GetAll()
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var allItems = myDb.Games.ToList();
                return allItems;
            }
        }

        public override Game GetById(int id)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemById = myDb.Games.Find(id);
                return itemById;
            }
        }

        public override bool Update(Game item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemToUpdateById = myDb.Games.Find(item.Id);
                myDb.Entry(itemToUpdateById).CurrentValues.SetValues(item);
                myDb.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// check if genre with specific Id has rows in the game table
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        public bool IsGenreIncluded(int genreId)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemById = myDb.Games.Where(x => x.GenreId==genreId).FirstOrDefault();
                return itemById==null?false:true;
            }
        }
        /// <summary>
        /// check if publisher with specific Id has rows in the game table
        /// </summary>
        /// <param name="publisherId"></param>
        /// <returns></returns>
        public bool IsPublisherIncluded(int publisherId)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var itemById = myDb.Games.Where(x => x.PublisherID == publisherId).FirstOrDefault();
                return itemById == null ? false : true;
            }
        }
        public bool IsNameAndYearUnique(Game item)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
                var searchResult = myDb.Games.Where(x => (x.GameName == item.GameName)&&(x.YearOfProduction==item.YearOfProduction)).FirstOrDefault();
                bool isUnique = searchResult == null ? true : false;
                return isUnique;
            }
        }
        public IEnumerable<Game> GetGameByPublisherLicense(int licenseNumber)
        {
            using (ComputerGamesEntities myDb = new ComputerGamesEntities())
            {
               // var result = myDb.Games.Join(myDb.Publishers, game => game.PublisherID, publisher => publisher, (game) => new { Name = game.GameName });

                var result = myDb.Games.Include(game=>game.Publisher).Where(publ => publ.Publisher.LicenseNumber == licenseNumber).ToList();

                return result;
            }
        }
    }
}
