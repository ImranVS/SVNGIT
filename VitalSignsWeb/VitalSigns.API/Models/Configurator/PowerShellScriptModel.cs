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

        [Newtonsoft.Json.JsonProperty("sub_types")]
        public List<string> SubTypes { get; set; }

        [Newtonsoft.Json.JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [Newtonsoft.Json.JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [Newtonsoft.Json.JsonProperty("user_id")]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("password")]
        public string Password { get; set; }

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

    public class PowerScriptsAuditModel
    {
        [Newtonsoft.Json.JsonProperty("script_path")]
        public string ScriptPath { get; set; }

        [Newtonsoft.Json.JsonProperty("script_name")]
        public string ScriptName { get; set; }

        [Newtonsoft.Json.JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [Newtonsoft.Json.JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [Newtonsoft.Json.JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [Newtonsoft.Json.JsonProperty("response")]
        public string Response { get; set; }

        [Newtonsoft.Json.JsonProperty("user_name")]
        public string UserName { get; set; }

        [Newtonsoft.Json.JsonProperty("user_id")]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("date_time_executed")]
        public DateTime DateTimeExecuted { get; set; }

        [Newtonsoft.Json.JsonProperty("parameters")]
        public List<Parameters> ParametersList { get; set; }

        public class Parameters : PowerShellScriptModel.Parameters
        {
        }
    }

}
