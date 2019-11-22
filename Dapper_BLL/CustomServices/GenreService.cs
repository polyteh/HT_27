using AutoMapper;
using Dapper_BLL.Entities;
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
    //custom logic for Genre
    //description string has limit: 600 characters max. Add and Update methods should check input data
    public class GenreService : BLLServices<BLLGenre, Genre>
    {
        private readonly int _maxDescriptionLenght = 600;
        //create instance of genre repository
        public GenreService()
        {
            this._curEFRep = new GenreRepositoryEF();
        }
        public override int Add(BLLGenre item)
        {
            bool isDescriptionValid = CheckDescriptionLenght(item.Description);
            //check description string lenght and return 0, if string is longer
            if (!isDescriptionValid)
            {
                Console.WriteLine($"Description cant be more than {_maxDescriptionLenght} characters");
                return 0;
            }
            // check if item with exists
            if (IsGenreNameExists(item))
            {
                Console.WriteLine($"Genre with same name already exists");
                return 0;
            }
            //if string lenght is shoter, than 600 - try to add item to base
            int res = base.Add(item);
            return res;
        }
        public override bool Update(BLLGenre item)
        {
            bool isDescriptionValid = CheckDescriptionLenght(item.Description);
            if (!isDescriptionValid)
            {
                Console.WriteLine($"Description cant be more than {_maxDescriptionLenght} characters");
                return false;
            }
            // check if item with exists
            if (IsGenreNameExists(item))
            {
                Console.WriteLine($"Genre with same name already exists");
                return false;
            }
            bool res = base.Update(item);
            return res;
        }
        public override bool Delete(BLLGenre item)
        {
            //return false, if we try to delete item, which doesnt exist in DB, or item, which has references
            if ((IsIdExists(item) == false) || (IsGenreIncluded(item)))
            {
                return false;
            }
            return base.Delete(item);
        }
        /// <summary>
        /// check string lenght, return true, if string lenght acceptable
        /// </summary>
        /// <param name="descriptionString"></param>
        /// <returns></returns>
        private bool CheckDescriptionLenght(string descriptionString)
        {
            if (descriptionString.Length > _maxDescriptionLenght)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// check if genre with specific name already exists
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsGenreNameExists(BLLGenre item)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BLLGenre, Genre>()).CreateMapper();
            var itemToAdd = mapper.Map<BLLGenre, Genre>(item);
            bool isGenreNameExist = (((GenreRepositoryEF)_curEFRep).IsNameExist(itemToAdd));
            return isGenreNameExist;
        }
        /// <summary>
        /// check, if genre has references in the games
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsGenreIncluded(BLLGenre item)
        {
            GameRepositoryEF gameRep = new GameRepositoryEF();
            bool isGenreIncluded = gameRep.IsGenreIncluded(item.Id);
            return isGenreIncluded;
        }
    }
}
