using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace VitalSigns.API.Models
{
    public class MaintainUsersModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("login_name")]
        public string LoginName { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("super_admin")]
        public string SuperAdmin { get; set; }

        [JsonProperty("configurator_access")]
        public bool ConfiguratorAccess { get; set; }

        [JsonProperty("console_command_access")]
        public bool ConsoleCommandAccess { get; set; }

    }
}




