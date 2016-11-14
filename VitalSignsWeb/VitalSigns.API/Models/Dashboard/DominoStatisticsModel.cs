using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class DominoStatisticsModel
    {
        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("total_mail_delivered")]
        public double? TotalMailDelivered { get; set; }

        [JsonProperty("avg_mail_delivery")]
        public double? AvgMailDelivery { get; set; }

        [JsonProperty("avg_server_availability_index")]
        public double? AvgServerAvailabilityIndex { get; set; }

        [JsonProperty("down_time")]
        public double? DownTime { get; set; }

        [JsonProperty("avg_memory")]
        public double? AvgMemory { get; set; }

        [JsonProperty("web_documents_opened")]
        public double? WebDocumentsOpened { get; set; }

        [JsonProperty("web_documents_created")]
        public double? WebDocumentsCreated { get; set; }

        [JsonProperty("web_database_opened")]
        public double? WebDatabaseOpened { get; set; }

        [JsonProperty("web_views_opened")]
        public double? WebViewsOpened { get; set; }

        [JsonProperty("web_commands_total")]
        public double? WebCommandsTotal { get; set; }

        [JsonProperty("http_session")]
        public double? HttpSession { get; set; }


    }
}
