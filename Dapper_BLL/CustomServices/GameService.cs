using AutoMapper;
using Dapper_BLL.Entities;
using Dapper_BLL.Interfaces;
using Dapper_DAL;
using Dapper_DAL.CustomRepository;
using Dapper_DAL.Entities;
using EF_Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_BLL.CustomServices
{
    public class GameService : BLLServices<BLLGame, Game>//, IGameService
    {
        //limitations for game definition
        private int _maxGaneNameLenght = 60;
        private int _minYear = 1980;
        public GameService()
        {
            //create instance of game repository
            this._curEFRep = new GameRepositoryEF();
        }
        public override int Add(BLLGame item)
        {
            //year of production and game name validation, return 0, if not valid
            bool isDescriptionValid = (CheckGameYear(item.YearOfProduction) && CheckGameName(item.GameName));
            if (!isDescriptionValid)
            {
                Console.WriteLine($"Wrong GameName or YearOfProduction characters");
                return 0;
            }
            //validation of navigation keys (if id of genre and publisher exist for new game), return 0, if not exist
            bool isIdPublisherAndGenreExists = IsIdPublisherAndGenreExists(item);
            if (!isIdPublisherAndGenreExists)
            {
                Console.WriteLine($"Wrong genre of publisher id. Add genre or publisher first");
                return 0;
            }
            //validation of unique combination of the game name and year
            if (!IsNameAndYearUnique(item))
            {
                Console.WriteLine($"Game with the same Name and year already exists");
                return 0;
            }

            //add item
            int res = base.Add(item);
            return res;
        }
        public override bool Update(BLLGame item)
        {
            bool isDescriptionValid = (CheckGameYear(item.YearOfProduction) && CheckGameName(item.GameName));
            if (!isDescriptionValid)
            {
                Console.WriteLine($"Wrong GameName or YearOfProduction characters");
                return false;
            }
            //validation of navigation key (if id of genre and publisher exist for new game)
            bool isIdPublisherAndGenreExists = IsIdPublisherAndGenreExists(item);
            if (!isIdPublisherAndGenreExists)
            {
                Console.WriteLine($"Wrong genre of publisher id. Add genre or publisher first");
                return false;
            }
            bool res =  base.Update(item);
            return res;
        }
        private bool CheckGameYear(int gameYear)
        {
            if ((gameYear < _minYear) && (gameYear > DateTime.Now.Year))
            {
                return false;
            }
            return true;
        }
        private bool CheckGameName(string gameName)
        {
            if (gameName.Length > _maxGaneNameLenght)
            {
                return false;
            }
            return true;
        }/// <summary>
        /// check if publisher and genre exists, before add new game
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsIdPublisherAndGenreExists(BLLGame item)
        {
            GenreRepositoryEF locGenreDapperRep = new GenreRepositoryEF();
            PublisherRepositoryEF locPublcDapperRep = new PublisherRepositoryEF();
            var gamePublisher = locPublcDapperRep.GetById(item.PublisherID);
            var gameGenre = locGenreDapperRep.GetById(item.GenreId);
            //check genre and publisher, if not exist return false         
            if ((gameGenre == null) || (gamePublisher == null))
            {
                return false;
            }
            return true;
        }

        public override bool Delete(BLLGame item)
        {
            if (IsIdExists(item) == false)
            {
                return false;
            }
            return base.Delete(item);
        }
        /// <summary>
        /// check if game name and year are unique before add new game
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsNameAndYearUnique(BLLGame item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BLLGame, Game>()).CreateMapper();
            var itemToCheck = mapper.Map<BLLGame, Game>(item);
            bool isUnique = ((GameRepositoryEF)_curEFRep).IsNameAndYearUnique(itemToCheck);
            return isUnique;
        }

        public IEnumerable<BLLGame> GetGameByPublisherLicense(int licenseNumber)
        {
            if (IsLicenseExists(licenseNumber))
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Game, BLLGame>()).CreateMapper();
                return mapper.Map<IEnumerable<Game>, List<BLLGame>>(((GameRepositoryEF)_curEFRep).GetGameByPublisherLicense(licenseNumber));
            }
            return null;
            
        }
        /// <summary>
        /// check if publisher with specific license exists, before query for its games
        /// </summary>
        /// <param name="licenseNumber"></param>
        /// <returns></returns>
        private bool IsLicenseExists(int licenseNumber)
        {
            PublisherRepositoryEF locPublcRep = new PublisherRepositoryEF();
            var publByLicense = locPublcRep.GetByPublisherLicense(licenseNumber);
            bool isLicenseExists = publByLicense != null ? true : false;
            return isLicenseExists;
        }
    }
}
