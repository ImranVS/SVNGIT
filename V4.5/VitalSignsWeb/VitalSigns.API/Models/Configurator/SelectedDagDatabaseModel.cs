using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
    public class SelectedDagDatabaseModel
    {
        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }

        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("database_name")]
        public string DatabaseName { get; set; }

        [JsonProperty("current_replay_queue")]
        public int? CurrentReplayQueue { get; set; }

        [JsonProperty("current_copy_queue")]       
        public int? CurrentCopyQueue { get; set; }

        [JsonProperty("replay_queue_threshold")]
        public int? ReplayQueueThreshold { get; set; }

        [JsonProperty("copy_queue_threshold")]
        public int? CopyQueueThreshold { get; set; }
        



    }
    
}
