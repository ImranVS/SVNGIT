using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class MsolUser
    {
        [JsonProperty("display_name", NullValueHandling =NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("user_principal_name", NullValueHandling = NullValueHandling.Ignore)]
        public string UserPrincipalName { get; set; }

        [JsonProperty("user_type", NullValueHandling = NullValueHandling.Ignore)]
        public string UserType { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("license", NullValueHandling = NullValueHandling.Ignore)]
        public string Licensed { get; set; }

        [JsonProperty("is_licensed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsLicensed { get; set; }

        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        public string Department { get; set; }

        [JsonProperty("strong_password_required", NullValueHandling = NullValueHandling.Ignore)]
        public bool? StrongPasswordRequired { get; set; }

        [JsonProperty("password_never_expires", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PasswordNeverExpires { get; set; }

        [JsonProperty("account_disabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AccountDisabled { get; set; }

        [JsonProperty("account_last_modified", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? AccountLastModified { get; set; }

        [JsonProperty("ad_last_sync", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ADLastSync { get; set; }

        [JsonProperty("old_sync", NullValueHandling = NullValueHandling.Ignore)]
        public bool?  OldSync { get; set; }
    }
}
