using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class NotificationsModel
    {
        [JsonProperty ("id")]
        public string ID { get; set; }

        [JsonProperty("notification_name")]
        public string NotificationName { get; set; }

        [JsonProperty("hours_destinations_id")]
        public string HoursDestinationsID { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("business_hours_id")]
        public string BusinessHoursId { get; set; }

        [JsonProperty("business_hours_type")]
        public string BusinessHoursType { get; set; }

        [JsonProperty("b_id")]
        public int BId { get; set; }

        [JsonProperty("send_via")]
        public string SendVia { get; set; }

        [JsonProperty("send_to")]
        public string SendTo { get; set; }

        [JsonProperty("copy_to")]
        public string CopyTo { get; set; }

        [JsonProperty("blind_copy_to")]
        public string BlindCopyTo { get; set; }

        [JsonProperty("persistent_notification")]
        public bool PersistentNotification { get; set; }

        [JsonProperty("escalation")]
        public string Escalation { get; set; }

    }

    public class NotificationDefinition
    {
        [JsonProperty("notifications")]
        public NotificationsModel Notifications { get; set; }

        [JsonProperty("selected_events")]
        public List<EventsModel> SelectedEvents { get; set; }

        [JsonProperty("selected_servers")]
        public List<ServersModel> SelectedServers { get; set; }
    }
}
