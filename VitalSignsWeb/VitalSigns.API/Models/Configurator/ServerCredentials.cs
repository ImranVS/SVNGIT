using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ServerCredentialsModel
    {


        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        //[JsonProperty("confirm_password")]
        //public string ConfirmPassword { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_modified")]
        public bool IsModified { get; set; }

    }
}
