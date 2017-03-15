using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class Office365Users
    {

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("principal_name")]
        public string PrincipalName { get; set; }

        [JsonProperty("user_type")]
        public string UserType { get; set; }

        [JsonProperty("strong_pwd_required")]
        public bool StrongPwdRequired { get; set; }

        [JsonProperty("pwd_never_expires")]
        public bool PwdNeverExpires { get; set; }
    }
}
