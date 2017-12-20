using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ExchangeCASModel
    {
        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("rpc")]
        public string RPC { get; set; }

        [JsonProperty("imap")]
        public string IMAP { get; set; }

        [JsonProperty("owa")]
        public string OWA { get; set; }

        [JsonProperty("pop3")]
        public string POP3 { get; set; }

        [JsonProperty("active_sync")]
        public string ActiveSync { get; set; }

        [JsonProperty("smtp")]
        public string SMTP { get; set; }

        [JsonProperty("outlook_anywhere")]
        public string OutlookAnywhere { get; set; }

        [JsonProperty("auto_discovery")]
        public string AutoDiscovery { get; set; }

        [JsonProperty("services")]
        public string Services { get; set; }
    }
}
