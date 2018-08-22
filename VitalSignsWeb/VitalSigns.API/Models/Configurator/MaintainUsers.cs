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

       
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("powerscript_roles")]
        public List<string> PowerScriptRoles { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("is_password_reset_required")]
        public bool IsPasswordResetRequired { get; set; }

        [JsonProperty("ad_user")]
        public bool AdUser { get; set; }


    }
}




