using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MigrateVitalSignsData.Model
{
    
    public class MapperData
    {
        //[JsonProperty("collection_name")]
        //public string CollectionName { get; set; }
        [JsonProperty("sql_query")]
        public string SQLQuery { get; set; }
        [JsonProperty("mappings")]
        public List<Mapping> Mappings { get; set; }
    }

    public class Mapping
    {
        [JsonProperty("sql_column")]
        public string SQLColumn { get; set; }

        [JsonProperty("mongodb_field")]
        public string MongoDbColumn { get; set; }

      
    }
}
