using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class AlertSettingsModel
    {
        [JsonProperty("primary_host_name")]
        public string PrimaryHostName { get; set; }

        [JsonProperty("primary_from")]
        public string PrimaryForm { get; set; }

        [JsonProperty("primary_user_id")]
        public string PrimaryUserId { get; set; }

        [JsonProperty("primary_port")]
        public int PrimaryPort { get; set; }

        [JsonProperty("primary_auth")]
        public bool PrimaryAuth { get; set; }

        [JsonProperty("primary_ssl")]
        public bool PrimarySSL { get; set; }

        [JsonProperty("primary_pwd")]
        public string PrimaryPwd { get; set; }

        [JsonProperty("primary_modified")]
        public bool PrimaryModified { get; set; }

        [JsonProperty("secondary_host_name")]
        public string SecondaryHostName { get; set; }

        [JsonProperty("secondary_from")]
        public string SecondaryForm { get; set; }

        [JsonProperty("secondary_user_id")]
        public string SecondaryUserId { get; set; }

        [JsonProperty("secondary_pwd")]
        public string SecondaryPwd { get; set; }

        [JsonProperty("secondary_modified")]
        public bool SecondaryModified { get; set; }

        [JsonProperty("secondary_port")]
        public string SecondaryPort { get; set; }

        [JsonProperty("secondary_auth")]
        public bool SecondaryAuth { get; set; }

        [JsonProperty("secondary_ssl")]
        public bool SecondarySSL { get; set; }

        [JsonProperty("sms_account_sid")]
        public string SmsAccountSid { get; set; }

        [JsonProperty("sms_auth_token")]
        public string SmsAuthToken { get; set; }

        [JsonProperty("sms_from")]
        public string SmsForm { get; set; }



        [JsonProperty("enable_persistent_alerting")]
        public bool EnablePersistentAlerting { get; set; }

        [JsonProperty("alert_interval")]
        public int AlertInterval { get; set; }

        [JsonProperty("alert_duration")]
        public int AlertDuration { get; set; }

        //[JsonProperty("e_mail")]
        //public string EMail { get; set; }

        [JsonProperty("enable_alert_limits")]
        public bool EnableAlertLimits { get; set; }

        [JsonProperty("total_maximum_alerts_per_definition")]
        public int TotalMaximumAlertsPerDefinition { get; set; }

        [JsonProperty("total_maximum_alerts_per_day")]
        public int TotalMaximumAlertsPerDay { get; set; }

        [JsonProperty("enable_SNMP_traps")]
        public bool EnableSNMPTraps { get; set; }

        [JsonProperty("host_name")]
        public string HostName { get; set; }

        [JsonProperty("alert_about_recurrences_only")]
        public bool AlertAboutRecurrencesOnly { get; set; }

        [JsonProperty("number_of_recurrences")]
        public int NumberOfRecurrences { get; set; }

        [JsonProperty("alerts_on")]
        public bool AlertsOn { get; set; }
    }
}
