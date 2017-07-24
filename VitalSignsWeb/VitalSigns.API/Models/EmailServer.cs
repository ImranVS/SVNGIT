using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class EmailServer
    {
        [JsonProperty("email_id")]
        public string emailId { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("email_hostName")]
        public string emailHostName { get; set; }

        [JsonProperty("email_userId")]
        public string emailUserId { get; set; }

        [JsonProperty("email_password")]
        public string emailPassword { get; set; }

        [JsonProperty("email_emailport")]
        public int emailPort { get; set; }

        [JsonProperty("email_emailSSL")]
        public string emailSSL { get; set; }

        [JsonProperty("Body")]
        public string Body { get; set; }

        [JsonProperty("Subject")]
        public string Subject { get; set; }

        [JsonProperty("email_type")]
        public string emailtype { get; set; }
    }
}
