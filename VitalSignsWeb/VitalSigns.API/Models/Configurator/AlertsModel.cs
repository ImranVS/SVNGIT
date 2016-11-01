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
    }
}
