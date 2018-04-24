using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Charts
{
    [BsonIgnoreExtraElements]
    public class DiskChart
    {


        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }


        [JsonProperty("series")]
        public ICollection<DiskSerie> Series { get; set; }
    }
}
