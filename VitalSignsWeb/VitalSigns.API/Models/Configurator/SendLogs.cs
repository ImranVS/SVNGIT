using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Configurator
{
    public class SendLogs
    {
      
        [JsonProperty("log_file_name")]
        public string LogFileName { get; set; }

  
    }

   
}
