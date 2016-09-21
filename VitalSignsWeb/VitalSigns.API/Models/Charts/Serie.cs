using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Charts
{
    public class Serie
    {
        [JsonProperty("title")]
        public string Title { get; set; }
       

        [JsonProperty("segments")]
        public ICollection<Segment> Segments { get; set; }
      

      
    }
}
