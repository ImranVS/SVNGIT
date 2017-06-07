using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using VitalSigns.API.Models.Charts;
using MongoDB.Bson.Serialization;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.Globalization;
using System.Dynamic;
using Microsoft.AspNet.Authorization;

namespace VitalSigns.API.Controllers
{
    [Authorize("Bearer")]
    [Route("services")]
    public class UtilsController : BaseController
    {
        private IRepository<StatsTranslation> statstranslationRepository;

        public string GetUserFriendlyStatName(string statname)
        {
            string translatedstat;
            statstranslationRepository = new Repository<StatsTranslation>(ConnectionString);
            FilterDefinition<StatsTranslation> filterdefST;

            translatedstat = statname;
            try
            {
                //exclude stat names with # to process Exchange queue stats correctly
                if (statname.IndexOf('@') != -1 && statname.IndexOf('#') == -1)
                {
                    statname = statname.Substring(0, statname.IndexOf('@'));
                }
                filterdefST = statstranslationRepository.Filter.Eq(x => x.StatName, statname);
                var result = statstranslationRepository.Find(filterdefST).ToList();
                if (result.Count > 0)
                {
                    translatedstat = result[0].TranslatedName;
                }
                return translatedstat;
            }
            catch (Exception exception)
            {
                return translatedstat;
            }
        }

        public bool isRPRWyattMachine()
        {
            Repository<License> licenseRepo = new Repository<License>(ConnectionString);
            try
            {
                return licenseRepo.Find(x => true).First().CompanyName == "RPRWyatt";
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
