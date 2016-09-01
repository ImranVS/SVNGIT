using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class TravelerHealth
    {

        [JsonProperty("_id")]
        public int? ID { get; set; }

        [JsonProperty("resource_constraint")]
        public string ResourceConstraint { get; set; }

        [JsonProperty("traveler_details")]
        public string TravelerDetails { get; set; }

        [JsonProperty("traveler_heartbeat")]
        public string TravelerHeartBeat { get; set; }

        [JsonProperty("traveler_servlet")]
        public string TravelerServlet { get; set; }


        [JsonProperty("traveler_users")]
        public int? TravelerUsers { get; set; }

        [JsonProperty("traveler_ha")]
        public Boolean? TravelerHA { get; set; }

        [JsonProperty("traveler_incremental_syncs")]
        public int? TravelerIncrementalSyncs { get; set; }


        [JsonProperty("http_status")]
        public string HttpStatus { get; set; }

        [JsonProperty("traveler_devices_api_status")]
        public string TravelerDevicesAPIStatus { get; set; }

        [JsonProperty("mail_server_name")]
        public string MailServerName { get; set; }

        [JsonProperty("date_updated")]
        public DateTime? DateUpdated { get; set; }

        [JsonProperty("000-001")]
        public int? Intervel1 { get; set; }

        [JsonProperty("001-002")]
        public int? Intervel2 { get; set; }

        [JsonProperty("002-005")]
        public int? Intervel3 { get; set; }

        [JsonProperty("005-010")]
        public int? Intervel4 { get; set; }

        [JsonProperty("010-030")]
        public int? Intervel5 { get; set; }

        [JsonProperty("030-060")]
        public int? Intervel6 { get; set; }

        [JsonProperty("060-120")]
        public int? Intervel7 { get; set; }

        [JsonProperty("120-INF")]
        public int? Intervel8 { get; set; }

     
    }
}

