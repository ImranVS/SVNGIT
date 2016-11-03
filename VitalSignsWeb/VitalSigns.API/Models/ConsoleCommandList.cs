using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ConsoleCommandList
    {
        [JsonProperty("server_name")]
        public string DeviceName { get; set; }

        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("submitter")]
        public string Submitter { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

       
    }
}
