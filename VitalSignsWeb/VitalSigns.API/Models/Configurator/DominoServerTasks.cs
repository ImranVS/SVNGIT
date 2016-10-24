using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
    public class DominoServerTasksModel
    {
        [JsonProperty("is_selected")]
        public bool? IsSelected { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("task_name")]       
        public string TaskName { get; set; }

        [JsonProperty("is_load")]
        public bool? IsLoad { get; set; }

        [JsonProperty("is_restart_asap")]
        public bool? IsRestartASAP { get; set; }

        [JsonProperty("is_resart_later")]
        public bool? IsResartLater { get; set; }

        [JsonProperty("is_disallow")]
        public bool? IsDisallow { get; set; }


    }

    public class DominoServerTasksValue
    {
        [JsonProperty("task_name")]
        public string TaskName { get; set; }

        [JsonProperty("is_load")]
        public bool IsLoad { get; set; }

        [JsonProperty("is_restart_asap")]
        public bool IsRestartASAP { get; set; }

        [JsonProperty("is_resart_later")]
        public bool IsResartLater { get; set; }

        [JsonProperty("is_disallow")]
        public bool IsDisallow { get; set; }

    }
}
