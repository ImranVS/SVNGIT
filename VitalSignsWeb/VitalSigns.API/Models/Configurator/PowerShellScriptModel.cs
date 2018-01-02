using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class PowerShellScriptModel
    {
        [Newtonsoft.Json.JsonProperty("path")]
        public string Path { get; set; }

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }

        [Newtonsoft.Json.JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [Newtonsoft.Json.JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [Newtonsoft.Json.JsonProperty("parameters")]
        public List<Parameters> ParametersList { get; set; }

        public class Parameters
        {
            [Newtonsoft.Json.JsonProperty("name")]
            public string Name { get; set; }

            [Newtonsoft.Json.JsonProperty("value")]
            public string Value { get; set; }
        }

    }
}
