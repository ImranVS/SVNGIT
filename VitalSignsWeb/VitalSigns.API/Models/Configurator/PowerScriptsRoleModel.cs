using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Configurator
{
    public class PowerScriptRole
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("file_paths")]
        public List<string> FilePaths;

        [JsonProperty("all_selected")]
        public bool AllSelected = false;
    }
}
