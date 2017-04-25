using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using MigrateVitalSignsData.Mappers;
using VSNext.Mongo.Entities;
using VSNext.Mongo.Repository;
using MongoDB.Driver;
using System.Data;

namespace MigrateVitalSignsData
{
    public class Program
    {
        public static void Main(string[] args)
        {

            if (args.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Syntax: MigrateVitalSignData <sql connection string> <mongo connection string>");

            }
            else
            {
                MappingHelper.SQLVitalsignsConnectionString = args[0];
                MappingHelper.SQLStatisticsConnectionString = args[0].Replace("Catalog=VitalSigns", "Catalog=VSS_Statistics");
                MappingHelper.MongoConnectionString = args[1];

                

                //Credentials
                Console.WriteLine("Processing Credentials");
                var credentials = new Mapper<Credentials>("Credentials.json").Map();
                Repository<Credentials> credentialsRepository = new Repository<Credentials>(MappingHelper.MongoConnectionString);
                if(credentials.Count > 0)
                    credentialsRepository.Insert(credentials);

                //MobileDevices
                Console.WriteLine("Processing Mobile Devices");
                var mobileDevices = new Mapper<MobileDevices>("MobileDevices.json").Map();
                Repository<MobileDevices> mobileDeviecesRepository = new Repository<MobileDevices>(MappingHelper.MongoConnectionString);
                if (mobileDevices.Count > 0)
                    mobileDeviecesRepository.Insert(mobileDevices);

                //Locations
                Console.WriteLine("Processing Locations");
                var locations = new Mapper<Location>("location.json").Map();
                Repository<Location> locationRepository = new Repository<Location>(MappingHelper.MongoConnectionString);
                if (locations.Count > 0)
                    locationRepository.Insert(locations);

                //License
                Console.WriteLine("Processing License");
                var license = new Mapper<License>("License.json").Map();
                Repository<License> licenseRepository = new Repository<License>(MappingHelper.MongoConnectionString);
                if (license.Count > 0)
                    licenseRepository.Insert(license);

                Console.WriteLine("Processing Nodes");
                var node = new Mapper<Nodes>("Node.json").Map();
                Repository<Nodes> nodeRepository = new Repository<Nodes>(MappingHelper.MongoConnectionString);
                if (node.Count > 0)
                    nodeRepository.Insert(node);

                Console.WriteLine("Processing Maintenance");
                var maintenance = new Mapper<Maintenance>("maintenance.json").Map();
                Repository<Maintenance> maintenanceRepository = new Repository<Maintenance>(MappingHelper.MongoConnectionString);
                if (maintenance.Count > 0)
                    maintenanceRepository.Insert(maintenance);

                //Console.WriteLine("Processing Credentials");
                //var ibm_connections_objects = new Mapper<IbmConnectionsObjects>("ibm_connections_object.json").Map();
                //Repository<IbmConnectionsObjects> connectionsRepository = new Repository<IbmConnectionsObjects>(MappingHelper.MongoConnectionString);
                // connectionsRepository.Insert(ibm_connections_objects);

                Console.WriteLine("Processing Settings");
                var namevalue = new Mapper<NameValue>("namevalue.json").Map();
                Repository<NameValue> nameValueRepository = new Repository<NameValue>(MappingHelper.MongoConnectionString);
                if (namevalue.Count > 0)
                    nameValueRepository.Insert(namevalue);
                    //foreach (var entity in namevalue)
                        //nameValueRepository.Replace(entity, new FilterDefinitionBuilder<NameValue>().Eq(x => x.Name, entity.Name), (new UpdateOptions() { IsUpsert = true }));


                Console.WriteLine("Processing Users");
                var users = new Mapper<Users>("Users.json").Map();
                Repository<Users> userRepository = new Repository<Users>(MappingHelper.MongoConnectionString);
                if (users.Count > 0)
                    userRepository.Insert(users);

                Console.WriteLine("Processing Consolidation Reports");
                var consolidationresult = new Mapper<ConsolidationResults>("ConsolidationResults.json").Map();
                Repository<ConsolidationResults> consolidatioresultRepository = new Repository<ConsolidationResults>(MappingHelper.MongoConnectionString);
                if (consolidationresult.Count > 0)
                    consolidatioresultRepository.Insert(consolidationresult);

                Console.WriteLine("Processing Domino Server Tasks");
                var dominoservertask = new Mapper<DominoServerTasks>("DominoServerTasks.json").Map();
                Repository<DominoServerTasks> dominoservertaskRepository = new Repository<DominoServerTasks>(MappingHelper.MongoConnectionString);
                if (dominoservertask.Count > 0)
                    dominoservertaskRepository.Insert(dominoservertask);



                Console.WriteLine("Processing Buiness Hours");
                var businessHours = new Mapper<BusinessHours>("business_hours.json").Map();
                string query = "SELECT [ID],[Type],[Starttime],[Duration],[UseType],[Issunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[Issaturday]  FROM [vitalsigns].[dbo].[HoursIndicator]";
                var hoursIndicatorTable = (MappingHelper.ExecuteQuery(query)).AsEnumerable().ToList();
                foreach (var businesshour in businessHours)
                {
                    var hourIndicatorRow = hoursIndicatorTable.Where(x => x.Field<string>(1) == businesshour.Name).Select(x => new
                    {
                        Sunday = x.Field<bool?>(5),
                        Monday = x.Field<bool?>(6),
                        Tuesday = x.Field<bool?>(7),
                        Wednesday = x.Field<bool?>(8),
                        Thursday = x.Field<bool?>(9),
                        Friday = x.Field<bool?>(10),
                        Saturday = x.Field<bool?>(11)


                    }).FirstOrDefault();

                    List<string> days = new List<string>();
                    if (hourIndicatorRow != null)
                    {
                        
                        if (hourIndicatorRow.Sunday.HasValue && hourIndicatorRow.Sunday.Value)
                            days.Add("Sunday");
                        if (hourIndicatorRow.Monday.HasValue && hourIndicatorRow.Monday.Value)
                            days.Add("Monday");
                        if (hourIndicatorRow.Tuesday.HasValue && hourIndicatorRow.Tuesday.Value)
                            days.Add("Tuesday");
                        if (hourIndicatorRow.Wednesday.HasValue && hourIndicatorRow.Wednesday.Value)
                            days.Add("Wednesday");
                        if (hourIndicatorRow.Thursday.HasValue && hourIndicatorRow.Thursday.Value)
                            days.Add("Thursday");
                        if (hourIndicatorRow.Friday.HasValue && hourIndicatorRow.Friday.Value)
                            days.Add("Friday");
                        if (hourIndicatorRow.Saturday.HasValue && hourIndicatorRow.Saturday.Value)
                            days.Add("Saturday");
                    }

                    //if (days.Count() > 0)
                    businesshour.Days = days.ToArray();

                }
                Repository<BusinessHours> business_hoursRepository = new Repository<BusinessHours>(MappingHelper.MongoConnectionString);
                if (businessHours.Count > 0)
                    business_hoursRepository.Insert(businessHours);

                    /*foreach (var entity in businessHours)
                        business_hoursRepository.Replace(entity, new FilterDefinitionBuilder<BusinessHours>().Eq(x => x.Name, entity.Name), (new UpdateOptions() { IsUpsert = true }));
                        */
                Console.WriteLine("Processing Server Data");
                MigrateData.ServerData.MigrateServerData();

                Console.WriteLine("Processing Cluster Datebase Details");
                var clusterDatabaseDetails = new Mapper<ClusterDatabaseDetails>("cluster_database_details.json").Map();
                Repository<ClusterDatabaseDetails> clusterDatabseRepository = new Repository<ClusterDatabaseDetails>(MappingHelper.MongoConnectionString);
                if (clusterDatabaseDetails.Count > 0)
                    clusterDatabseRepository.Insert(clusterDatabaseDetails);

                Console.WriteLine("Processing Alerts");
                AlertsData.MigrateAlertsData();

                Console.WriteLine("Processing Traveler Summary Stats");
                var travelerSummaryStats = new Mapper<TravelerStatusSummary>("traveler_summary_stats.json").Map();
                if (travelerSummaryStats.Count() > 0)
                {
                    Repository<TravelerStatusSummary> travelerSummaryStatsRepository = new Repository<TravelerStatusSummary>(MappingHelper.MongoConnectionString);
                    travelerSummaryStatsRepository.Insert(travelerSummaryStats);
                }

                Console.WriteLine("Processing Traveler Data Store");
                var travelerDataStore = new Mapper<TravelerDTS>("traveler_data_store.json").Map();
                Repository<TravelerDTS> travelerDataStoreRepository = new Repository<TravelerDTS>(MappingHelper.MongoConnectionString);
                if (travelerDataStore.Count > 0)
                    travelerDataStoreRepository.Insert(travelerDataStore);

                Console.WriteLine("Processing Mobile Device Translations");
                var mobileDeviceTranslations = new Mapper<MobileDeviceTranslations>("mobile_device_translations.json").Map();
                Repository<MobileDeviceTranslations> mobileDeviceTranslationsRepository = new Repository<MobileDeviceTranslations>(MappingHelper.MongoConnectionString);
                if (mobileDeviceTranslations.Count > 0)
                    mobileDeviceTranslationsRepository.Insert(mobileDeviceTranslations);

                //Console.WriteLine("Processing Credentials");
                var sharepointWebTrafficDailyStats = new Mapper<SharePointWebTrafficDailyStatistics>("sharepoint_webtraffic_dailystats.json").Map();
                if (sharepointWebTrafficDailyStats.Count() > 0)
                {
                    Repository<SharePointWebTrafficDailyStatistics> sharepointWebTrafficDailyStatsRepository = new Repository<SharePointWebTrafficDailyStatistics>(MappingHelper.MongoConnectionString);
                   // sharepointWebTrafficDailyStatsRepository.Insert(sharepointWebTrafficDailyStats);
                }

                //var deviceDailyStats = new Mapper<DailyStatistics>("daily_statistics.json").Map();
                //if (deviceDailyStats.Count() > 0)
                //{
                //    Repository<DailyStatistics> deviceDailyStatsRepository = new Repository<DailyStatistics>(MappingHelper.MongoConnectionString);
                //    deviceDailyStatsRepository.Insert(deviceDailyStats);
                //}

                //var deviceSummaryStats = new Mapper<SummaryStatistics>("daily_statistics.json").Map();
                //if (deviceSummaryStats.Count() > 0)
                //{
                //    Repository<SummaryStatistics> deviceSummaryStatsRepository = new Repository<SummaryStatistics>(MappingHelper.MongoConnectionString);
                //    deviceSummaryStatsRepository.Insert(deviceSummaryStats);
                //}


                Console.WriteLine("Processing Scripts");
                var scripts = new Mapper<Scripts>("Scripts.json").Map();
                Repository<Scripts> scriptsRepository = new Repository<Scripts>(MappingHelper.MongoConnectionString);
                if (scripts.Count > 0)
                    scriptsRepository.Insert(scripts);

                Console.WriteLine("Processing Log Files");
                var logfile = new Mapper<LogFile>("logfile.json").Map();
                Repository<LogFile> logFileRepository = new Repository<LogFile>(MappingHelper.MongoConnectionString);
                if (logfile.Count > 0)
                    logFileRepository.Insert(logfile);

                Console.WriteLine("Processing Traveler Stats");
                Repository<TravelerStats> travelerStatsRepository = new Repository<TravelerStats>(MappingHelper.MongoConnectionString);
                var travelerstats = new Mapper<TravelerStats>("travelerstats.json").Map();
                Repository<Server> serverRepository = new Repository<Server>(MappingHelper.MongoConnectionString);
                var servers = serverRepository.Collection.AsQueryable().ToList();
                foreach (var travelerstat in travelerstats)
                {
                    var serverid = servers.FirstOrDefault(x => x.DeviceName == travelerstat.DeviceId);
                    if (serverid != null)
                    {
                        travelerstat.DeviceId = serverid.Id;
                    }
                }
                if (travelerstats.Count > 0)
                    travelerStatsRepository.Insert(travelerstats);

                Console.WriteLine("Processing Summary Stats");
                var summaryStats = new Mapper<SummaryStatistics>("SummaryStats.json", vitalsignsDatabase: false).Map();
                foreach(var summaryStat in summaryStats)
                {
                    try
                    {
                        string ID = servers.Where(x => x.DeviceName == summaryStat.DeviceName && x.DeviceType == summaryStat.DeviceType).First().Id;
                        summaryStat.DeviceId = ID;
                    }
                    catch(Exception ex)
                    {

                    }
                }
                Repository<SummaryStatistics> summaryStatsRepository = new Repository<SummaryStatistics>(MappingHelper.MongoConnectionString);
                if (summaryStats.Count > 0)
                    summaryStatsRepository.Insert(summaryStats);

                

            }
        }
    }
}
