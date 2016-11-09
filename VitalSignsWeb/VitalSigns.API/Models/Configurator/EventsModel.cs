using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class EventsModel
    {
        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

    }

    public class RecurringEvents
    {
        [JsonProperty("alert_settings")]
        public AlertSettingsModel AlertSettings { get; set; }

        [JsonProperty("selected_events")]
        public List<EventsModel> SelectedEvents { get; set; }


    }
}
