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

        [JsonProperty("device")]
        public string DeviceName { get; set; }

        [JsonProperty ("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("alert_type")]
        public string AlertType { get; set; }

        [JsonProperty("event_detected_sent")]
        public DateTime? EventDetectedSent { get; set; }

        [JsonProperty("event_dismissed")]
        public DateTime? EventDismissed { get; set; }

        [JsonProperty("notification_sent_to")]
        public string NotificationSentTo { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("event_detected")]
        public DateTime? EventDetected { get; set; }
    }
}
