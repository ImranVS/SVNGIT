using MigrateVitalSignsData.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSNext.Mongo.Entities;
using VSNext.Mongo.Repository;
using MongoDB.Driver;
using System.Data;
using MongoDB.Bson;

namespace MigrateVitalSignsData.MigrateData
{
    public static class DailyStatsData
    {
        static Repository<DailyStatistics> dailyStatsRepository = new Repository<DailyStatistics>(MappingHelper.MongoConnectionString);
        static Repository<Server> eventsmasterRepository = new Repository<Server>(MappingHelper.MongoConnectionString);
        public static void MigrateAlertsData()
        {
            var daminoDailyStatistics = new Mapper<DailyStatistics>("Domino_statistics.json").Map();
            if (daminoDailyStatistics.Count() > 0)
            {
                dailyStatsRepository.Insert(daminoDailyStatistics);
            }

            var deviceDailyStatistics = new Mapper<DailyStatistics>("Device_statistics.json").Map();
            if (deviceDailyStatistics.Count() > 0)
            {
                dailyStatsRepository.Insert(deviceDailyStatistics);
            }
        }
    }
}
