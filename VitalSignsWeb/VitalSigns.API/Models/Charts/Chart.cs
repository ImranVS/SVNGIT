using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Charts
{
    public class Chart
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("y_axis_title")]
        public string YAxisTitle { get; set; }

        [JsonProperty("series")]
        public ICollection<Serie> Series { get; set; }
    }
}
