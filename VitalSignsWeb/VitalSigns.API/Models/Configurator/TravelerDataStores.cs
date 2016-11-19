using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models.Configurator
{
    public class TravelerDataStoresModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("traveler_service_pool_name")]
        public string TravelerServicePoolName { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("data_store")]
        public string DataStore { get; set; }

        [JsonProperty("database_name")]
        public string DatabaseName { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("integrated_security")]
        public int? IntegratedSecurity { get; set; }

        [JsonProperty("test_scan_server")]
        public string TestScanServer { get; set; }

        [JsonProperty("used_by_servers")]
        public string UsedByServers { get; set; }

    }
}
