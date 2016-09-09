using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class MonitoredTasks
    {

        //[JsonProperty("_id")]
        //public int? ID { get; set; }

        [JsonProperty("task_name")]
        public string TaskName { get; set; }

        [JsonProperty("monitored")]
        public Boolean? Monitored { get; set; }

        [JsonProperty("primary_status")]
        public string PrimaryStatus { get; set; }

        [JsonProperty("secondary_status")]
        public string SecondaryStatus { get; set; }


        [JsonProperty("last_updated")]
        public string LastUpdated { get; set; }

       
        

     
    }
}

