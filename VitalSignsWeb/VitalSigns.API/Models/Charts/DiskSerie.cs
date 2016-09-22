using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Charts
{
    public class DiskSerie
    {
       
        [JsonProperty("label")]
       
        public string Label { get; set; }

        [JsonProperty("value")]

        public List<double> Value { get; set; }



    }
}
