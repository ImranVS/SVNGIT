using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class AlertsModel
    {
        [JsonProperty ("id")]
        public string ID { get; set; }

        [JsonProperty("enable_persitent_alerting")]
        public bool EnablePersitentAlerting { get; set; }

        [JsonProperty("alert_interval")]
        public int AlertInterval { get; set; }

        [JsonProperty("alert_duration")]
        public int AlertDuration { get; set; }

        [JsonProperty("e_mail")]
        public string EMail { get; set; }

        [JsonProperty("enable_alert_limits")]
        public bool EnableAlertLimits { get; set; }

        [JsonProperty("total_maximum_alerts_per_definition")]
        public int TotalMaximumAlertsPerDefinition { get; set; }

        [JsonProperty ("total_maximum_alerts_per_day")]
        public int TotalMaximumAlertsPerDay { get; set; }

        [JsonProperty("enable_SNMP_traps")]
        public bool EnableSNMPTraps { get; set; }

        [JsonProperty("host_name")]
        public string HostName { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty ("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("alert_type")]
        public string AlertType { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("event_detected_sent")]
        public DateTime? EventDetectedSent { get; set; }

        [JsonProperty("event_dismissed")]
        public DateTime? EventDismissed { get; set; }

        [JsonProperty("notification_sent_to")]
        public string NotificationSentTo { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("primary_host_name")]
        public string PrimaryHostName { get; set; }

        [JsonProperty("primary_from")]
        public string PrimaryForm{ get; set; }

        [JsonProperty("primary_user_id")]
        public string PrimaryUserId { get; set; }

        [JsonProperty("primary_port")]
        public string PrimaryPort { get; set; }

        [JsonProperty("primary_auth")]
        public string PrimaryAuth { get; set; }

        [JsonProperty("primary_ssl")]
        public string PrimarySSL { get; set; }

        [JsonProperty("secondary_host_name")]
        public string SecondaryHostName { get; set; }

        [JsonProperty("secondary_from")]
        public string SecondaryForm { get; set; }

        [JsonProperty("secondary_user_id")]
        public string SecondaryUserId { get; set; }

        [JsonProperty("secondary_pwd")]
        public string SecondaryPwd { get; set; }

        [JsonProperty("secondary_port")]
        public string SecondaryPort { get; set; }

        [JsonProperty("secondary_auth")]
        public string SecondaryAuth { get; set; }

        [JsonProperty("secondary_auth")]
        public string secondarySSL { get; set; }

        [JsonProperty("sms_account_sid")]
        public string SmsAccountSid { get; set; }

        [JsonProperty("sms_auth_token")]
        public string SmsAuthToken { get; set; }

        [JsonProperty("sms_from")]
        public string SmsForm { get; set; }







    }
}
