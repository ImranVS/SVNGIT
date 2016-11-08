using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class MailDeliveryStatusModel
    {
        [JsonProperty("device_name")]
        public string DeviceName { get; set; }


        [JsonProperty("pending_mail")]
        public int? PendingMail { get; set; }

        [JsonProperty("dead_mail")]
        public int? DeadMail { get; set; }

        [JsonProperty("held_mail")]
        public int? HeldMail { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("last_updated")]
        public DateTime? LastUpdated { get; set; }
     
        [JsonProperty("StatusCode")]
        public string StatusCode { get; set; }
        
    }
}
