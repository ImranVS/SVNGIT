using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class NotificationsModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("notification_name")]
        public string NotificationName { get; set; }

        [JsonProperty("hours_destinations_id")]
        public string HoursDestinationsID { get; set; }

        [JsonProperty("business_hours_id")]
        public string BusinessHoursId { get; set; }

        [JsonProperty("business_hours_type")]
        public string BusinessHoursType { get; set; }

        [JsonProperty("b_id")]
        public int? BId { get; set; }

        [JsonProperty("send_via")]
        public string SendVia { get; set; }

        [JsonProperty("send_via_list")]
        public List<string> SendViaList { get; set; }

        [JsonProperty("send_to")]
        public string SendTo { get; set; }

        [JsonProperty("send_to_list")]
        public List<string> SendToList { get; set; }

        [JsonProperty("copy_to")]
        public string CopyTo { get; set; }

        [JsonProperty("blind_copy_to")]
        public string BlindCopyTo { get; set; }

        [JsonProperty("script_name")]
        public string ScriptName { get; set; }

        [JsonProperty("script_command")]
        public string ScriptCommand { get; set; }

        [JsonProperty("script_location")]
        public string ScriptLocation { get; set; }

        [JsonProperty("persistent_notification")]
        public bool? PersistentNotification { get; set; }

        [JsonProperty("escalation_id")]
        public string EscalationId { get; set; }

        [JsonProperty("interval")]
        public int? Interval { get; set; }

        [JsonProperty("business_hours_ids")]
        public List<string> BusinessHoursIds { get; set; }

        [JsonProperty("is_selected_hour")]
        public List<bool> IsSelectedHour { get; set; }

        [JsonProperty("escalation_ids")]
        public List<string> EscalationIds { get; set; }

        [JsonProperty("is_selected_escalation")]
        public List<bool> IsSelectedEscalation { get; set; }

        [JsonProperty("event_ids")]
        public List<string> EventIds { get; set; }

        [JsonProperty("is_selected_event")]
        public List<bool> IsSelectedEvent { get; set; }

        [JsonProperty("server_ids")]
        public List<string> ServerIds { get; set; }

        [JsonProperty("is_selected_server")]
        public List<bool> IsSelectedServer { get; set; }

        [JsonProperty("server_objects")]
        public List<ServerObjects> ServerObjects { get; set; }

    }

    public class ServerObjects
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("is_selected")]
        public bool? IsSelected { get; set; }

        [JsonProperty("collection_name")]
        public string CollectionName { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("location_name")]
        public string LocationName { get; set; }
        
    }

    public class NotificationChildIds
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class HourDefinition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_selected_hour")]
        public bool IsSelectedHour { get; set; }

        [JsonProperty("send_via")]
        public string SendVia { get; set; }

        [JsonProperty("send_to")]
        public string SendTo { get; set; }

        [JsonProperty("copy_to")]
        public string CopyTo { get; set; }

        [JsonProperty("blind_copy_to")]
        public string BlindCopyTo { get; set; }

        [JsonProperty("script_name")]
        public string ScriptName { get; set; }

        [JsonProperty("script_command")]
        public string ScriptCommand { get; set; }

        [JsonProperty("script_location")]
        public string ScriptLocation { get; set; }

        [JsonProperty("business_hours_type")]
        public string BusinessHoursType { get; set; }

        [JsonProperty("persistent_notification")]
        public bool? PersistentNotification { get; set; }

        [JsonProperty("interval")]
        public int? Interval { get; set; }
    }

    public class EventDefinition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_selected_event")]
        public bool IsSelectedEvent { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("notification_on_repeat")]
        public bool NotificationOnRepeat { get; set; }
    }

    public class ServerDefinition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_selected_server")]
        public bool IsSelectedServer { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class ScriptDefinition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("script_name")]
        public string ScriptName { get; set; }

        [JsonProperty("script_command")]
        public string ScriptCommand { get; set; }

        [JsonProperty("script_location")]
        public string ScriptLocation { get; set; }

    }

    public class EventFilter
    {
        [JsonProperty("event_types")]
        public List<string> EventTypes { get; set; }

        [JsonProperty("device_types")]
        public List<string> DeviceTypes { get; set; }

        [JsonProperty("device_names")]
        public List<string> DeviceNames { get; set; }

    }
}
